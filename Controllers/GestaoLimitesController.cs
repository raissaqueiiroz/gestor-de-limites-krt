using gestor_de_limitres_krt.Helpers;
using gestor_de_limitres_krt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace gestor_de_limitres_krt.Controllers
{
    public class GestaoLimitesController : Controller
    {
        private readonly IGestaoLimitesRepository _gestaoLimitesRepository;

        public GestaoLimitesController(IGestaoLimitesRepository gestaoLimitesRepository)
        {
            _gestaoLimitesRepository = gestaoLimitesRepository;
        }

        public IActionResult AtualizaLimite()
        {
            return View();
        }

        public IActionResult CadastraLimite()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastraLimite(string documento, string numeroConta, string numeroAgencia, string limitePix)
        {

            var limitePixDouble = DynamoDBHelper.ConverteStringToDouble(limitePix);
            var documentoFormatado = DynamoDBHelper.FormataChaveValorCpfBanco(documento);
            var skvalue = DynamoDBHelper.FormataChaveValorIsTransacao(documento);

            var clienteCadastrado = await _gestaoLimitesRepository.CadastraLimite(documentoFormatado, skvalue, numeroConta, numeroAgencia, limitePixDouble);

            if (clienteCadastrado == null)
            {
                ViewBag.Message = "Cliente já existe";
                return View();

            } else
            {
                ViewBag.Message = "Limite criado com sucesso!";
                return View(clienteCadastrado);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizaLimite(string documento, string novoLimite)
        {

            var novoLimitePixDouble = DynamoDBHelper.ConverteStringToDouble(novoLimite);
            var documentoFormatado = DynamoDBHelper.FormataChaveValorCpfBanco(documento);
            var skValue = DynamoDBHelper.FormataChaveValorIsTransacao(documento);


            var clienteAtualizado = await _gestaoLimitesRepository.AtualizaLimite(documentoFormatado, skValue, novoLimitePixDouble);

            if (clienteAtualizado == null)
            {
                ViewBag.Message = "Cliente não existe";
                return View();

            } else
            {
                ViewBag.Message = "Limite atualizado com sucesso!";
                return View(clienteAtualizado);
            }

        }
    }
}

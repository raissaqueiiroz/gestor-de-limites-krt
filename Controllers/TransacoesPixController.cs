using gestor_de_limitres_krt.Helpers;
using gestor_de_limitres_krt.Models;
using gestor_de_limitres_krt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace gestor_de_limitres_krt.Controllers
{
    public class TransacoesPixController : Controller
    {
        private readonly ITransacoesPixRepository _transacoesPixRepository;

        public TransacoesPixController(ITransacoesPixRepository transacoesPixRepository)
        {
            _transacoesPixRepository = transacoesPixRepository;
        }

        public IActionResult RegistrarTransacaoPix()
        {
            return View();
        }

        public IActionResult ObterTransacoesPix()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarTransacaoPix(string documentoMandante, string documentoRecebedor, string valorDaTransferencia)
        {
            var valorDaTransferenciaToDouble = DynamoDBHelper.ConverteStringToDouble(valorDaTransferencia);
            var documentoFormatadoMandante = DynamoDBHelper.FormataChaveValorCpfBanco(documentoMandante);
            var documentoFormatadoRecebedor = DynamoDBHelper.FormataChaveValorCpfBanco(documentoRecebedor);
            var skvalue = DynamoDBHelper.FormataChaveValorIsTransacao(documentoMandante);

            var responseTransacao = await _transacoesPixRepository.RegistrarTransacaoPix(documentoFormatadoMandante, skvalue, documentoFormatadoRecebedor, valorDaTransferenciaToDouble);

            if (responseTransacao == null)
            {
                ViewBag.Message = "Erro ao registrar transação.";
                return View();

            } else
            {
                ViewBag.Message = "Transação feita com sucesso!";
                return View(responseTransacao);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ObterTransacoesPix(string documento)
        {
            var documentoFormatado = DynamoDBHelper.FormataChaveValorCpfBanco(documento);
            var skvalue = DynamoDBHelper.FormataChaveValorIsTransacao(documento);

            var listaTransacoes = await _transacoesPixRepository.ObterTransacoesPix(documentoFormatado, skvalue);

            if (listaTransacoes == null)
            {
                ViewBag.Message = "Erro ao retornar transação.";
                return View();

            }
            else
            {
                ViewBag.Message = "Lista de transações:";
                return View(listaTransacoes);
            }
        }
    }
}

using Amazon.DynamoDBv2.DataModel;
using gestor_de_limitres_krt.Helpers;
using gestor_de_limitres_krt.Models;
using gestor_de_limitres_krt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace gestor_de_limitres_krt.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly IRegistrosRepository _clienteRepository;

        public RegistrosController(IRegistrosRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public IActionResult BuscarRegistro()
        {
            return View();
        }

        public IActionResult DeletarRegistro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarRegistro(string documento)
        {
            var skvalue = DynamoDBHelper.FormataChaveValorIsTransacao(documento);
            var documentoFormatado = DynamoDBHelper.FormataChaveValorCpfBanco(documento);

            var cliente = await _clienteRepository.BuscarRegistro(documentoFormatado, skvalue);

            if (cliente != null)
            {
                ViewBag.Message = "Cliente retornado com sucesso!";
                return View(cliente);
            }
            else
            {
                ViewBag.Message = "Cliente não encontrado.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletarRegistro(string documento)
        {
            var skvalue = DynamoDBHelper.FormataChaveValorIsTransacao(documento);
            var documentoFormatado = DynamoDBHelper.FormataChaveValorCpfBanco(documento);

            var clienteDeletado = await _clienteRepository.DeletarRegistro(documentoFormatado, skvalue);

            if (clienteDeletado == null)
            {
                ViewBag.Message = "Erro ao deletar cliente.";
                return View();
            } else
            {
                ViewBag.Message = "Cliente deletado com sucesso.";
                return View();
            }
        }
    }
}

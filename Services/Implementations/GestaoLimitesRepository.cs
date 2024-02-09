using Amazon.DynamoDBv2.DataModel;
using gestor_de_limitres_krt.Helpers;
using gestor_de_limitres_krt.Models;
using gestor_de_limitres_krt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace gestor_de_limitres_krt.Services.Implementations
{
    public class GestaoLimitesRepository : IGestaoLimitesRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly ILogger _logger;

        public GestaoLimitesRepository(IDynamoDBContext context, ILogger<IGestaoLimitesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GestorDeLimitesModel> CadastraLimite(string documento, string skValue, string numeroConta, string numeroAgencia, double limitePix)
        {
            try
            {
                var cliente = await _context.LoadAsync<GestorDeLimitesModel>(documento, skValue);

                if (cliente != null) return null; 
                else
                {
                    var novoCliente = new GestorDeLimitesModel
                    {
                        Documento = documento,
                        Consulta = skValue,
                        Numero_Agencia = numeroAgencia,
                        Numero_Conta = numeroConta,
                        Limite_Pix = limitePix,
                    };

                    await _context.SaveAsync(novoCliente);

                    return novoCliente;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao cadastrar cliente. {ex.Message}");
                return null;
            }
        }

        public async Task<GestorDeLimitesModel> AtualizaLimite(string documento, string skValue, double novoLimite)
        {
            try
            {
                var cliente = await _context.LoadAsync<GestorDeLimitesModel>(documento, skValue);

                if (cliente != null)
                {
                    cliente.Limite_Pix = novoLimite;

                    await _context.SaveAsync(cliente);

                    return cliente;
                }
                else
                {
                    return cliente;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar limite do cliente. {ex.Message}");
                return null;
            }
        }
    }
}

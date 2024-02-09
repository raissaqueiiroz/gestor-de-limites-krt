using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using gestor_de_limitres_krt.Helpers;
using gestor_de_limitres_krt.Models;
using gestor_de_limitres_krt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace gestor_de_limitres_krt.Services.Implementations
{
    public class TransacoesPixRepository : ITransacoesPixRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly ILogger _logger;

        public TransacoesPixRepository(IDynamoDBContext context, ILogger<ITransacoesPixRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GestorDeLimitesModel> RegistrarTransacaoPix(string documento, string skValue, string documentoRecebedor, double valorTransferencia)
        {
            try
            {
                var clienteMandante = await _context.LoadAsync<GestorDeLimitesModel>(documento, skValue);

                if (clienteMandante != null)
                {

                    if (clienteMandante.Limite_Pix.HasValue)
                    {
                        double novoLimite = DynamoDBHelper.CalculaLimiteTransferencia(valorTransferencia, clienteMandante.Limite_Pix.Value);

                        if (novoLimite >= 0)
                        {
                            clienteMandante.Limite_Pix = novoLimite;

                            await _context.SaveAsync(clienteMandante);

                            await RegistraTransacaoComSucesso(clienteMandante, documentoRecebedor, valorTransferencia);

                            return clienteMandante;
                        }
                        else
                        {
                            await RegistraTransacaoComFalha(clienteMandante, valorTransferencia);

                            return null;
                            throw new Exception("Transferência negada: limite de transações PIX excedido.");
                        }
                    }
                    else
                    {
                        return null;
                        throw new Exception("Transferência negada: cliente sem limite definido.");
                    }
                }
                else
                {
                    return null;
                    throw new Exception("Transferência negada: cliente não encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao cadastrar cliente. {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<GestorDeLimitesModel>> ObterTransacoesPix(string documento, string skValue)
        {
            try
            {
                var clienteExiste = await _context.LoadAsync<GestorDeLimitesModel>(documento, skValue);

                if (clienteExiste == null) return null;
                else
                {

                    QueryFilter filter = new QueryFilter("documento", QueryOperator.Equal, $"{documento}");
                    filter.AddCondition("consulta", QueryOperator.BeginsWith, "TRANSACAO");

                    var config = new QueryOperationConfig
                    {
                        Filter = filter,
                    };

                    var asyncSearch = _context.FromQueryAsync<GestorDeLimitesModel>(config);
                    var results = await asyncSearch.GetRemainingAsync();
                    return results;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar limite do cliente. {ex.Message}");
                return null;
            }
        }

        public async Task RegistraTransacaoComSucesso(GestorDeLimitesModel mandanteTransacao, string recebedorTransacao, double valorTransacao)
        {
            var skvalue = DynamoDBHelper.FormataChaveValorIsTransacao("transacao");
            var currentDate = DateTime.Now.ToString();

            var novaTransacao = new GestorDeLimitesModel
            {
                Documento = mandanteTransacao.Documento,
                Consulta = skvalue,
                Data_Transacao = currentDate,
                Status_Transacao = "SUCESSO",
                Valor_Transacao = valorTransacao,
                Destino_Transacao = recebedorTransacao,
            };

            await _context.SaveAsync(novaTransacao);
        }

        public async Task RegistraTransacaoComFalha(GestorDeLimitesModel cliente, double valorTransacao)
        {
            var skvalue = DynamoDBHelper.FormataChaveValorIsTransacao("transacao");
            var currentDate = DateTime.Now.ToString();

            var novaTransacao = new GestorDeLimitesModel
            {
                Documento = cliente.Documento,
                Consulta = skvalue,
                Data_Transacao = currentDate,
                Status_Transacao = "FALHA",
                Valor_Transacao = valorTransacao,
            };

            await _context.SaveAsync(novaTransacao);
        }
    }
}

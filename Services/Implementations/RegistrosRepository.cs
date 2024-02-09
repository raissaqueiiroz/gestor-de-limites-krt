using Amazon.DynamoDBv2.DataModel;
using gestor_de_limitres_krt.Models;
using gestor_de_limitres_krt.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace gestor_de_limitres_krt.Services.Implementations
{
    public class RegistrosRepository : IRegistrosRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly ILogger _logger;

        public RegistrosRepository(IDynamoDBContext context, ILogger<IRegistrosRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GestorDeLimitesModel> BuscarRegistro(string documento, string skValue)
        {
            try
            {
                var cliente = await _context.LoadAsync<GestorDeLimitesModel>(documento, skValue);
                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar cliente. {ex.Message}");
                return null;
            }
        }

        public async Task<GestorDeLimitesModel> DeletarRegistro(string documento, string skValue)
        {
            try
            {
                var cliente = await _context.LoadAsync<GestorDeLimitesModel>(documento, skValue);

                if (cliente != null)
                {
                    await _context.DeleteAsync<GestorDeLimitesModel>(documento, skValue);
                    return cliente;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar cliente. {ex.Message}");
                return null;
            }
        }
    }
}

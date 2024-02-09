using gestor_de_limitres_krt.Models;
using Microsoft.AspNetCore.Mvc;

namespace gestor_de_limitres_krt.Services.Interfaces
{
    public interface IGestaoLimitesRepository
    {
        Task<GestorDeLimitesModel> CadastraLimite(string documento, string skValue, string numeroConta, string numeroAgencia, double limitePix);
        Task<GestorDeLimitesModel> AtualizaLimite(string documento, string skValue, double novoLimite);
    }
}

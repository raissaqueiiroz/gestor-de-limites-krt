using gestor_de_limitres_krt.Models;
using Microsoft.AspNetCore.Mvc;

namespace gestor_de_limitres_krt.Services.Interfaces
{
    public interface IRegistrosRepository
    {
        Task<GestorDeLimitesModel> BuscarRegistro(string documento, string skValue);
        Task<GestorDeLimitesModel> DeletarRegistro(string documento, string skValue);
    }
}


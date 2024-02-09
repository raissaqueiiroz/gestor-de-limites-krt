using gestor_de_limitres_krt.Models;

namespace gestor_de_limitres_krt.Services.Interfaces
{
    public interface ITransacoesPixRepository
    {
        Task<GestorDeLimitesModel> RegistrarTransacaoPix(string documento, string skValue, string documentoRecebedor, double valorTransferencia);
        Task<IEnumerable<GestorDeLimitesModel>> ObterTransacoesPix(string documento, string skValue);
    }
}

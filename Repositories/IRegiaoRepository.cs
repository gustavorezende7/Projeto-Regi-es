using RegioesApi.Models;

namespace RegioesApi.Repositories
{
    public interface IRegiaoRepository
    {
        Task<IEnumerable<Regiao>> ListarAsync(bool incluirInativas = false);
        Task<Regiao?> BuscarPorIdAsync(int id);
        Task<bool> ExisteComMesmoNomeAsync(string uf, string nome, int? ignorarId = null);
        Task CriarAsync(Regiao regiao);
        Task AtualizarAsync(Regiao regiao);
    }
}

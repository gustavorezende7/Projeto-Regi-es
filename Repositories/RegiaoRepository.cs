using Microsoft.EntityFrameworkCore;
using RegioesApi.Data;
using RegioesApi.Models;

namespace RegioesApi.Repositories
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private readonly AppDbContext _context;

        public RegiaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Regiao>> ListarAsync(bool incluirInativas = false)
        {
            if (incluirInativas)
                return await _context.Regioes.OrderBy(r => r.UF).ThenBy(r => r.Nome).ToListAsync();

            return await _context.Regioes.Where(r => r.Ativo).OrderBy(r => r.UF).ThenBy(r => r.Nome).ToListAsync();
        }

        public async Task<Regiao?> BuscarPorIdAsync(int id)
        {
            return await _context.Regioes.FindAsync(id);
        }

        public async Task<bool> ExisteComMesmoNomeAsync(string uf, string nome, int? ignorarId = null)
        {
            var consulta = _context.Regioes
                .Where(r => r.UF.ToLower() == uf.ToLower() && r.Nome.ToLower() == nome.ToLower());

            if (ignorarId.HasValue)
                consulta = consulta.Where(r => r.Id != ignorarId.Value);

            return await consulta.AnyAsync();
        }

        public async Task CriarAsync(Regiao regiao)
        {
            _context.Regioes.Add(regiao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Regiao regiao)
        {
            _context.Regioes.Update(regiao);
            await _context.SaveChangesAsync();
        }
    }
}

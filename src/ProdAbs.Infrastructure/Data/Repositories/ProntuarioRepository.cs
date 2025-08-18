using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using System.Threading.Tasks;

namespace ProdAbs.Infrastructure.Data.Repositories
{
    public class ProntuarioRepository : IProntuarioRepository
    {
        private readonly AppDbContext _context;

        public ProntuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Prontuario prontuario)
        {
            await _context.Prontuarios.AddAsync(prontuario);
        }

        public async Task<Prontuario> GetByIdAsync(Guid id)
        {
            return await _context.Prontuarios.FindAsync(id);
        }

        public Task UpdateAsync(Prontuario prontuario)
        {
            _context.Prontuarios.Update(prontuario);
            return Task.CompletedTask;
        }
    }
}

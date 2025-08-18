using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

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
    }
}

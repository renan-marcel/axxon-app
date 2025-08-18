using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdAbs.Infrastructure.Data.Repositories
{
    public class TipoDeDocumentoRepository : ITipoDeDocumentoRepository
    {
        private readonly AppDbContext _context;

        public TipoDeDocumentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TipoDocumento tipoDocumento)
        {
            await _context.TiposDeDocumento.AddAsync(tipoDocumento);
        }

        public async Task<TipoDocumento> GetByIdAsync(Guid id)
        {
            return await _context.TiposDeDocumento.FindAsync(id);
        }

        public async Task<List<TipoDocumento>> ListAsync()
        {
            return await _context.TiposDeDocumento.ToListAsync();
        }
    }
}

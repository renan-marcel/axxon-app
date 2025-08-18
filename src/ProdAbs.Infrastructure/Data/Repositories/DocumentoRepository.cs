using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.Infrastructure.Data.Repositories
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly AppDbContext _context;

        public DocumentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Documento documento)
        {
            await _context.Documentos.AddAsync(documento);
        }

        public async Task<Documento> GetByIdAsync(Guid id)
        {
            return await _context.Documentos.FindAsync(id);
        }
    }
}

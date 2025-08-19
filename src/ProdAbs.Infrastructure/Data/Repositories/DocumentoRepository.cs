using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProdAbs.Infrastructure.Data.Repositories
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public DocumentoRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(Documento? documento)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Documentos.Add(documento);
            await context.SaveChangesAsync();
        }

        public async Task<Documento?> GetByIdAsync(Guid id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Documentos.FindAsync(id);
        }
    }
}
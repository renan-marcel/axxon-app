using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.Infrastructure.Data.Repositories;

public class DocumentoRepository : IDocumentoRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public DocumentoRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Documento documento)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        dbContext.Documentos.Add(documento);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Documento> GetByIdAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await dbContext.Documentos.FindAsync(id).AsTask();
    }
}

using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.Infrastructure.Data.Repositories;

public class TipoDeDocumentoRepository : ITipoDeDocumentoRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public TipoDeDocumentoRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(TipoDocumento tipoDocumento)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        dbContext.TiposDeDocumento.Add(tipoDocumento);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TipoDocumento> GetByIdAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await dbContext.TiposDeDocumento.FindAsync(id).AsTask();
    }

    public async Task<List<TipoDocumento>> ListAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await dbContext.TiposDeDocumento.ToListAsync();
    }
}
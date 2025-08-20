using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.Infrastructure.Data.Repositories;

public class TipoDeDocumentoRepository : ITipoDeDocumentoRepository
{
    private readonly AppDbContext _dbContext;

    public TipoDeDocumentoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(TipoDocumento tipoDocumento)
    {
        _dbContext.TiposDeDocumento.Add(tipoDocumento);
        return _dbContext.SaveChangesAsync();
    }

    public Task<TipoDocumento> GetByIdAsync(Guid id)
    {
        return _dbContext.TiposDeDocumento.FindAsync(id).AsTask();
    }

    public Task<List<TipoDocumento>> ListAsync()
    {
        return _dbContext.TiposDeDocumento.ToListAsync();
    }
}
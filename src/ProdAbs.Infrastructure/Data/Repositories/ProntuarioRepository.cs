using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.Infrastructure.Data.Repositories;

public class ProntuarioRepository : IProntuarioRepository
{
    private readonly AppDbContext _dbContext;

    public ProntuarioRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(Prontuario prontuario)
    {
        _dbContext.Prontuarios.Add(prontuario);
        return _dbContext.SaveChangesAsync();
    }

    public Task<Prontuario> GetByIdAsync(Guid id)
    {
        return _dbContext.Prontuarios.FindAsync(id).AsTask();
    }

    public Task UpdateAsync(Prontuario prontuario)
    {
        _dbContext.Prontuarios.Update(prontuario);
        return _dbContext.SaveChangesAsync();
    }
}
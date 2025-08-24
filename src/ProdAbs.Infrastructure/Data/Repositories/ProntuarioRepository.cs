using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.Infrastructure.Data.Repositories;

public class ProntuarioRepository : IProntuarioRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public ProntuarioRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Prontuario prontuario)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        dbContext.Prontuarios.Add(prontuario);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Prontuario> GetByIdAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await dbContext.Prontuarios.FindAsync(id).AsTask();
    }

    public async Task UpdateAsync(Prontuario prontuario)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        dbContext.Prontuarios.Update(prontuario);
        await dbContext.SaveChangesAsync();
    }
}
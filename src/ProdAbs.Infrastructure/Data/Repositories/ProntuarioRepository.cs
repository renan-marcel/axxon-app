using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;

namespace ProdAbs.Infrastructure.Data.Repositories
{
    public class ProntuarioRepository : IProntuarioRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public ProntuarioRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(Prontuario prontuario)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Prontuarios.Add(prontuario);
            await context.SaveChangesAsync();
        }

        public async Task<Prontuario?> GetByIdAsync(Guid id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Prontuarios.FindAsync(id).AsTask();
        }

        public async Task UpdateAsync(Prontuario prontuario)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Prontuarios.Update(prontuario);
            await context.SaveChangesAsync();
        }
    }
}
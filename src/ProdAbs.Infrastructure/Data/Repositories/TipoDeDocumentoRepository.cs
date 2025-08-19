using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProdAbs.Infrastructure.Data.Repositories
{
    public class TipoDeDocumentoRepository : ITipoDeDocumentoRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public TipoDeDocumentoRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(TipoDocumento tipoDocumento)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.TiposDeDocumento.Add(tipoDocumento);
            await context.SaveChangesAsync();
        }

        public async Task<TipoDocumento?> GetByIdAsync(Guid id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.TiposDeDocumento.FindAsync(id).AsTask();
        }

        public async Task<List<TipoDocumento>> ListAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.TiposDeDocumento.ToListAsync();
        }
    }
}
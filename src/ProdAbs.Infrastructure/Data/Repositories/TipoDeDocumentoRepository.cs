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
}
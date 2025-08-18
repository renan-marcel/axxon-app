using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProdAbs.Infrastructure.Data.Repositories
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly AppDbContext _dbContext;

        public DocumentoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Documento documento)
        {
            _dbContext.Documentos.Add(documento);
            return _dbContext.SaveChangesAsync();
        }

        public Task<Documento> GetByIdAsync(Guid id)
        {
            return _dbContext.Documentos.FindAsync(id).AsTask();
        }
    }
}
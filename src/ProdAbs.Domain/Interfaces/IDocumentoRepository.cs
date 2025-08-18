
using ProdAbs.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ProdAbs.Domain.Interfaces
{
    public interface IDocumentoRepository
    {
        Task<Documento> GetByIdAsync(Guid id);
        Task AddAsync(Documento documento);
    }
}

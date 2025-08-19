
using ProdAbs.Domain.Entities;

namespace ProdAbs.Domain.Interfaces
{
    public interface IDocumentoRepository
    {
        Task<Documento?> GetByIdAsync(Guid id);
        Task AddAsync(Documento? documento);
    }
}

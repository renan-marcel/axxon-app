using ProdAbs.Domain.Entities;

namespace ProdAbs.Domain.Interfaces;

public interface ITipoDeDocumentoRepository
{
    Task<TipoDocumento> GetByIdAsync(Guid id);
    Task<List<TipoDocumento>> ListAsync();
    Task AddAsync(TipoDocumento tipoDocumento);
}
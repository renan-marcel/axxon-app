
using ProdAbs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdAbs.Domain.Interfaces
{
    public interface ITipoDeDocumentoRepository
    {
        Task<TipoDocumento> GetByIdAsync(Guid id);
        Task<List<TipoDocumento>> ListAsync();
        Task AddAsync(TipoDocumento tipoDocumento);
    }
}

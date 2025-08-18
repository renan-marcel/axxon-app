
using ProdAbs.Domain.Entities;

namespace ProdAbs.Domain.Interfaces
{
    public interface IProntuarioRepository
    {
        Task<Prontuario> GetByIdAsync(Guid id);
        Task AddAsync(Prontuario prontuario);
        Task UpdateAsync(Prontuario prontuario);
    }
}

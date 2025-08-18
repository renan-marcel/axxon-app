
using ProdAbs.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ProdAbs.Domain.Interfaces
{
    public interface IProntuarioRepository
    {
        Task<Prontuario> GetByIdAsync(Guid id);
        Task AddAsync(Prontuario prontuario);
        Task UpdateAsync(Prontuario prontuario);
    }
}

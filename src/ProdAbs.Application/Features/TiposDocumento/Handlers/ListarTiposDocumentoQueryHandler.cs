using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.TiposDocumento.Queries;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.TiposDocumento.Handlers;

public class ListarTiposDocumentoQueryHandler : IRequestHandler<ListarTiposDocumentoQuery, Result<List<TipoDocumentoDetalhesDTO>>>
{
    private readonly ITipoDeDocumentoRepository _repository;

    public ListarTiposDocumentoQueryHandler(ITipoDeDocumentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<TipoDocumentoDetalhesDTO>>> Handle(ListarTiposDocumentoQuery request, CancellationToken cancellationToken)
    {
        // Placeholder for MVP
        await Task.CompletedTask;
        var dtos = new List<TipoDocumentoDetalhesDTO>
        {
            new() { Id = Guid.NewGuid(), Nome = "Contrato Social", Campos = new List<string> { "CNPJ", "Razao Social" } },
            new() { Id = Guid.NewGuid(), Nome = "RG", Campos = new List<string> { "Nome", "CPF" } }
        };
        return Result.Success(dtos);
    }
}

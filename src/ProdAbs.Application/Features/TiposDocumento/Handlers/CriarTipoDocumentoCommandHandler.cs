using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.TiposDocumento.Commands;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.TiposDocumento.Handlers;

public class CriarTipoDocumentoCommandHandler : IRequestHandler<CriarTipoDocumentoCommand, Result<TipoDocumentoDetalhesDTO>>
{
    private readonly ITipoDeDocumentoRepository _repository;

    public CriarTipoDocumentoCommandHandler(ITipoDeDocumentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<TipoDocumentoDetalhesDTO>> Handle(CriarTipoDocumentoCommand request, CancellationToken cancellationToken)
    {
        // Placeholder for MVP
        await Task.CompletedTask;
        var dto = new TipoDocumentoDetalhesDTO { Id = Guid.NewGuid(), Nome = request.Nome, Campos = request.Campos };
        return Result.Ok(dto);
    }
}

using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.Prontuarios.Commands;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.Prontuarios.Handlers;

public class CriarProntuarioCommandHandler : IRequestHandler<CriarProntuarioCommand, Result<ProntuarioResumoDTO>>
{
    private readonly IProntuarioRepository _repository;

    public CriarProntuarioCommandHandler(IProntuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ProntuarioResumoDTO>> Handle(CriarProntuarioCommand request, CancellationToken cancellationToken)
    {
        // Placeholder for MVP
        await Task.CompletedTask;
        var dto = new ProntuarioResumoDTO
        {
            Id = Guid.NewGuid(),
            IdentificadorEntidade = request.IdentificadorEntidade,
            TipoProntuario = request.TipoProntuario,
            QuantidadeDocumentos = 0
        };
        return Result.Ok(dto);
    }
}

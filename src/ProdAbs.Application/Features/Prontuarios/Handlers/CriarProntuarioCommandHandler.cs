using MediatR;
using ProdAbs.Application.Features.Prontuarios.Commands;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Handlers;

public class CriarProntuarioCommandHandler : IRequestHandler<CriarProntuarioCommand, Result<Guid>>
{
    private readonly IProntuarioRepository _prontuarioRepository;

    public CriarProntuarioCommandHandler(IProntuarioRepository prontuarioRepository)
    {
        _prontuarioRepository = prontuarioRepository;
    }

    public async Task<Result<Guid>> Handle(CriarProntuarioCommand request, CancellationToken cancellationToken)
    {
        var prontuario = new Prontuario(
            Guid.NewGuid(),
            request.IdentificadorEntidade,
            request.TipoProntuario
        );

        await _prontuarioRepository.AddAsync(prontuario);

        return Result.Ok(prontuario.Id);
    }
}
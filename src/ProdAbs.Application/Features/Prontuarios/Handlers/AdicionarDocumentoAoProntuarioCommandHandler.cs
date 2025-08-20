using MediatR;
using ProdAbs.Application.Features.Prontuarios.Commands;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Handlers;

public class
    AdicionarDocumentoAoProntuarioCommandHandler : IRequestHandler<AdicionarDocumentoAoProntuarioCommand, Result>
{
    private readonly IProntuarioRepository _prontuarioRepository;

    public AdicionarDocumentoAoProntuarioCommandHandler(IProntuarioRepository prontuarioRepository)
    {
        _prontuarioRepository = prontuarioRepository;
    }

    public async Task<Result> Handle(AdicionarDocumentoAoProntuarioCommand request, CancellationToken cancellationToken)
    {
        var prontuario = await _prontuarioRepository.GetByIdAsync(request.ProntuarioId);

        if (prontuario == null) return Result.Fail("Prontuário não encontrado.");

        if (prontuario.DocumentoIds.Contains(request.DocumentoId))
            return Result.Fail("Documento já associado a este prontuário.");

        prontuario.DocumentoIds.Add(request.DocumentoId);
        await _prontuarioRepository.UpdateAsync(prontuario);

        return Result.Ok();
    }
}
using MediatR;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Commands;

public class AdicionarDocumentoAoProntuarioCommandHandler : IRequestHandler<AdicionarDocumentoAoProntuarioCommand, Result>
{
    private readonly IProntuarioRepository _prontuarioRepository;
    private readonly IDocumentoRepository _documentoRepository;

    public AdicionarDocumentoAoProntuarioCommandHandler(IProntuarioRepository prontuarioRepository, IDocumentoRepository documentoRepository)
    {
        _prontuarioRepository = prontuarioRepository;
        _documentoRepository = documentoRepository;
    }

    public async Task<Result> Handle(AdicionarDocumentoAoProntuarioCommand request, CancellationToken cancellationToken)
    {
        // Placeholder for MVP
        await Task.CompletedTask;
        return Result.Success();
    }
}

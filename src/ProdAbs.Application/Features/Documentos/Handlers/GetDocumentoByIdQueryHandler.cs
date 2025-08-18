using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.Documentos.Queries;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Documentos.Handlers;

public class GetDocumentoByIdQueryHandler : IRequestHandler<GetDocumentoByIdQuery, Result<DocumentoDTO>>
{
    private readonly IDocumentoRepository _repository;

    public GetDocumentoByIdQueryHandler(IDocumentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<DocumentoDTO>> Handle(GetDocumentoByIdQuery request, CancellationToken cancellationToken)
    {
        // Placeholder for MVP
        await Task.CompletedTask;
        var dto = new DocumentoDTO
        {
            Id = request.Id,
            TipoDeDocumentoId = Guid.NewGuid(),
            NomeArquivoOriginal = "sample.pdf",
            Formato = "application/pdf",
            TamanhoEmBytes = 12345,
            Status = "Ativo",
            Versao = 1,
            DicionarioDeCamposValores = new Dictionary<string, string> { { "Cliente", "Empresa XYZ" } }
        };
        return Result.Ok(dto);
    }
}

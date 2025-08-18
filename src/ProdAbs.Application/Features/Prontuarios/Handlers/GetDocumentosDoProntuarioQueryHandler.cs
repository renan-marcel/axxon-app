using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.Prontuarios.Queries;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Handlers;

public class GetDocumentosDoProntuarioQueryHandler : IRequestHandler<GetDocumentosDoProntuarioQuery, Result<List<DocumentoDTO>>>
{
    private readonly IProntuarioRepository _repository;

    public GetDocumentosDoProntuarioQueryHandler(IProntuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<DocumentoDTO>>> Handle(GetDocumentosDoProntuarioQuery request, CancellationToken cancellationToken)
    {
        // Placeholder for MVP
        await Task.CompletedTask;
        var dtos = new List<DocumentoDTO>
        {
            new() { Id = Guid.NewGuid(), NomeArquivoOriginal = "doc1.pdf", Formato = "application/pdf" },
            new() { Id = Guid.NewGuid(), NomeArquivoOriginal = "doc2.jpg", Formato = "image/jpeg" }
        };
        return Result.Success(dtos);
    }
}

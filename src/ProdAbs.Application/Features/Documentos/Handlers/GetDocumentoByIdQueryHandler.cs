using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.Documentos.Queries;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Documentos.Handlers;

public class GetDocumentoByIdQueryHandler : IRequestHandler<GetDocumentoByIdQuery, Result<DocumentoDTO>>
{
    private readonly IDocumentoRepository _documentoRepository;

    public GetDocumentoByIdQueryHandler(IDocumentoRepository documentoRepository)
    {
        _documentoRepository = documentoRepository;
    }

    public async Task<Result<DocumentoDTO>> Handle(GetDocumentoByIdQuery request, CancellationToken cancellationToken)
    {
        var documento = await _documentoRepository.GetByIdAsync(request.Id);

        if (documento == null) return Result.Fail<DocumentoDTO>("Documento não encontrado.");

        var dto = new DocumentoDTO
        {
            Id = documento.Id,
            TipoDeDocumentoId = documento.TipoDeDocumentoId,
            NomeArquivoOriginal = documento.NomeArquivoOriginal,
            Formato = documento.Formato,
            TamanhoEmBytes = documento.TamanhoEmBytes,
            HashValor = documento.HashValor,
            Versao = documento.Versao,
            DicionarioDeCamposValores = documento.DicionarioDeCamposValores
        };

        return Result.Ok(dto);
    }
}
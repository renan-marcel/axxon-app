using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.Prontuarios.Queries;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Handlers;

public class
    GetDocumentosDoProntuarioQueryHandler : IRequestHandler<GetDocumentosDoProntuarioQuery, Result<List<DocumentoDTO>>>
{
    private readonly IDocumentoRepository _documentoRepository;
    private readonly IProntuarioRepository _prontuarioRepository;

    public GetDocumentosDoProntuarioQueryHandler(IProntuarioRepository prontuarioRepository,
        IDocumentoRepository documentoRepository)
    {
        _prontuarioRepository = prontuarioRepository;
        _documentoRepository = documentoRepository;
    }

    public async Task<Result<List<DocumentoDTO>>> Handle(GetDocumentosDoProntuarioQuery request,
        CancellationToken cancellationToken)
    {
        var prontuario = await _prontuarioRepository.GetByIdAsync(request.ProntuarioId);

        if (prontuario == null) return Result.Fail<List<DocumentoDTO>>("Prontuário não encontrado.");

        var documentos = new List<DocumentoDTO>();
        foreach (var docId in prontuario.DocumentoIds)
        {
            var documento = await _documentoRepository.GetByIdAsync(docId);
            if (documento != null)
                documentos.Add(new DocumentoDTO
                {
                    Id = documento.Id,
                    TipoDeDocumentoId = documento.TipoDeDocumentoId,
                    NomeArquivoOriginal = documento.NomeArquivoOriginal,
                    Formato = documento.Formato,
                    TamanhoEmBytes = documento.TamanhoEmBytes,
                    HashValor = documento.HashValor,
                    Versao = documento.Versao,
                    DicionarioDeCamposValores = documento.DicionarioDeCamposValores
                });
        }

        return Result.Ok(documentos);
    }
}
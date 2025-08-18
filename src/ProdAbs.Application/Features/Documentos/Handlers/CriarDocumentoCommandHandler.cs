using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.Documentos.Commands;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.Documentos.Handlers;

public class CriarDocumentoCommandHandler : IRequestHandler<CriarDocumentoCommand, Result<DocumentoDTO>>
{
    private readonly IDocumentoRepository _documentoRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IPublisher _publisher;

    public CriarDocumentoCommandHandler(IDocumentoRepository documentoRepository, IFileStorageService fileStorageService, IPublisher publisher)
    {
        _documentoRepository = documentoRepository;
        _fileStorageService = fileStorageService;
        _publisher = publisher;
    }

    public async Task<Result<DocumentoDTO>> Handle(CriarDocumentoCommand request, CancellationToken cancellationToken)
    {
        var hash = await HashUtility.CalculateSha256Async(request.FileStream);
        request.FileStream.Position = 0;

        var uploadResult = await _fileStorageService.UploadAsync(request.FileStream, request.FileName, request.ContentType);
        if (uploadResult.IsFailure)
        {
            return Result.Fail<DocumentoDTO>(uploadResult.Error);
        }
        
        // Placeholder for MVP
        var dto = new DocumentoDTO
        {
            Id = Guid.NewGuid(),
            TipoDeDocumentoId = request.TipoDocumentoId,
            NomeArquivoOriginal = request.FileName,
            Formato = request.ContentType,
            TamanhoEmBytes = request.FileStream.Length,
            Status = "Criado",
            Versao = 1,
            DicionarioDeCamposValores = request.DicionarioDeCamposValores
        };
        return Result.Ok(dto);
    }
}

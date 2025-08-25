using MassTransit;
using MediatR;
using ProdAbs.Application.Features.Documentos.Commands;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using ProdAbs.SharedKernel.Events;

namespace ProdAbs.Application.Features.Documentos.Handlers;

public class CriarDocumentoCommandHandler : IRequestHandler<CriarDocumentoCommand, Result<Guid>>
{
    private readonly IDocumentoRepository _documentoRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ITopicProducer<Guid, IDocumentoCriadoEvent> _producer;
    private readonly TimeProvider _timeProvider;

    public CriarDocumentoCommandHandler(
        IDocumentoRepository documentoRepository,
        IFileStorageService fileStorageService,
        ITopicProducer<Guid, IDocumentoCriadoEvent> producer, TimeProvider timeProvider)
    {
        _documentoRepository = documentoRepository;
        _fileStorageService = fileStorageService;
        _producer = producer;
        _timeProvider = timeProvider;
    }

    public async Task<Result<Guid>> Handle(CriarDocumentoCommand request, CancellationToken cancellationToken)
    {
        using var stream = request.File.OpenReadStream();

        //var fileId = Guid.NewGuid();

        //var filename = $"{fileId}{Path.GetExtension(request.File.FileName)}";

        var hash = await HashUtility.CalculateSha256Async(stream);

        stream.Seek(0, SeekOrigin.Begin);
        var uploadResult =
            await _fileStorageService.UploadAsync(stream, request.File.FileName, request.File.ContentType);

        if (uploadResult.IsFailure) return Result.Fail<Guid>(uploadResult.Error);

        var documento = new Documento(
            Guid.NewGuid(),
            request.TipoDocumentoId,
            uploadResult.Value,
            request.File.Length,
            "SHA256",
            hash,
            request.File.FileName,
            request.File.ContentType,
            _timeProvider.GetUtcNow(),
            new Dictionary<string, string>()
        );

        await _documentoRepository.AddAsync(documento);

        // Publish event via MassTransit if available, otherwise via IMediator
        var evt = new DocumentoCriadoEvent
        {
            Id = documento.Id,
            TipoDocumentoId = documento.TipoDeDocumentoId,
            StorageLocation = documento.StorageLocation,
            TamanhoEmBytes = documento.TamanhoEmBytes,
            HashValor = documento.HashValor
        };

        await _producer.Produce(documento.Id, evt, cancellationToken);

        return Result.Ok(documento.Id);
    }
}
using MediatR;
using ProdAbs.Application.Features.Documentos.Commands;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using ProdAbs.SharedKernel.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.Documentos.Handlers
{
    public class CriarDocumentoCommandHandler : IRequestHandler<CriarDocumentoCommand, Result<System.Guid>>
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMediator _mediator;

        public CriarDocumentoCommandHandler(
            IDocumentoRepository documentoRepository,
            IFileStorageService fileStorageService,
            IMediator mediator)
        {
            _documentoRepository = documentoRepository;
            _fileStorageService = fileStorageService;
            _mediator = mediator;
        }

        public async Task<Result<Guid>> Handle(CriarDocumentoCommand request, CancellationToken cancellationToken)
        {
            using var stream = request.File.OpenReadStream();

            // Calculate hash
            var hash = await HashUtility.CalculateSha256Async(stream);

            // Upload file
            stream.Seek(0, SeekOrigin.Begin); // Reset stream position for upload
            var uploadResult = await _fileStorageService.UploadAsync(stream, request.File.FileName, request.File.ContentType);

            if (uploadResult.IsFailure)
            {
                return Result.Fail<Guid>(uploadResult.Error);
            }

            var documento = new Documento(
                Guid.NewGuid(),
                request.TipoDocumentoId,
                uploadResult.Value,
                request.File.Length,
                "SHA256",
                hash,
                request.File.FileName,
                request.File.ContentType,
                new Dictionary<string, string>() // request.DicionarioDeCamposValores
            );

            await _documentoRepository.AddAsync(documento);

            //// Publish event
            //await _mediator.Publish(new DocumentoCriadoEvent
            //{
            //    Id = documento.Id,
            //    TipoDocumentoId = documento.TipoDeDocumentoId,
            //    StorageLocation = documento.StorageLocation,
            //    TamanhoEmBytes = documento.TamanhoEmBytes,
            //    HashValor = documento.HashValor
            //}, cancellationToken);

            return Result.Ok(documento.Id);
        }
    }
}
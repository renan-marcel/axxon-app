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

        public async Task<Result<System.Guid>> Handle(CriarDocumentoCommand request, CancellationToken cancellationToken)
        {
            // Calculate hash
            request.FileStream.Seek(0, SeekOrigin.Begin); // Reset stream position
            var hash = await HashUtility.CalculateSha256Async(request.FileStream);

            // Upload file
            request.FileStream.Seek(0, SeekOrigin.Begin); // Reset stream position again for upload
            var uploadResult = await _fileStorageService.UploadAsync(request.FileStream, request.FileName, request.ContentType);

            if (uploadResult.IsFailure)
            {
                return Result.Fail<System.Guid>(uploadResult.Error);
            }

            var documento = new Documento
            {
                StorageLocation = uploadResult.Value,
                TamanhoEmBytes = request.FileStream.Length,
                HashTipo = "SHA256",
                HashValor = hash,
                NomeArquivoOriginal = request.FileName,
                Formato = request.ContentType,
                TipoDeDocumentoId = request.TipoDocumentoId,
                DicionarioDeCamposValores = request.DicionarioDeCamposValores,
                Versao = 1 // Initial version
            };

            await _documentoRepository.AddAsync(documento);

            // Publish event
            await _mediator.Publish(new DocumentoCriadoEvent
            {
                Id = documento.Id,
                TipoDocumentoId = documento.TipoDeDocumentoId,
                StorageLocation = documento.StorageLocation,
                TamanhoEmBytes = documento.TamanhoEmBytes,
                HashValor = documento.HashValor
            }, cancellationToken);

            return Result.Ok(documento.Id);
        }
    }
}
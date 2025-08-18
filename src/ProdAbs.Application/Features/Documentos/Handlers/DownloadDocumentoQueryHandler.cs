using MediatR;
using ProdAbs.Application.Features.Documentos.Queries;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.Documentos.Handlers
{
    public class DownloadDocumentoQueryHandler : IRequestHandler<DownloadDocumentoQuery, Result<Stream>>
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IFileStorageService _fileStorageService;

        public DownloadDocumentoQueryHandler(IDocumentoRepository documentoRepository, IFileStorageService fileStorageService)
        {
            _documentoRepository = documentoRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Stream>> Handle(DownloadDocumentoQuery request, CancellationToken cancellationToken)
        {
            var documento = await _documentoRepository.GetByIdAsync(request.Id);

            if (documento == null)
            {
                return Result.Fail<Stream>("Documento não encontrado.");
            }

            var fileStreamResult = await _fileStorageService.GetAsync(documento.StorageLocation);

            if (fileStreamResult.IsFailure)
            {
                return Result.Fail<Stream>(fileStreamResult.Error);
            }

            return Result.Ok(fileStreamResult.Value);
        }
    }
}
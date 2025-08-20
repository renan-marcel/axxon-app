using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.Documentos.Queries;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Documentos.Handlers
{
    public class DownloadDocumentoQueryHandler : IRequestHandler<DownloadDocumentoQuery, Result<FileDownloadDTO>>
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IFileStorageService _fileStorageService;

        public DownloadDocumentoQueryHandler(IDocumentoRepository documentoRepository, IFileStorageService fileStorageService)
        {
            _documentoRepository = documentoRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<FileDownloadDTO>> Handle(DownloadDocumentoQuery request, CancellationToken cancellationToken)
        {
            var documento = await _documentoRepository.GetByIdAsync(request.Id);

            if (documento == null)
            {
                return Result.Fail<FileDownloadDTO>("Documento não encontrado.");
            }

            var fileStreamResult = await _fileStorageService.GetAsync(documento.StorageLocation);

            if (fileStreamResult.IsFailure)
            {
                return Result.Fail<FileDownloadDTO>(fileStreamResult.Error);
            }

            var fileDownloadDto = new FileDownloadDTO
            {
                File = fileStreamResult.Value,
                FileName = documento.NomeArquivoOriginal,
                ContentType = documento.Formato
            };

            return Result.Ok(fileDownloadDto);
        }
    }
}
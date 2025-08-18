using MediatR;
using ProdAbs.Application.Features.Documentos.Queries;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.Documentos.Handlers;

public class DownloadDocumentoQueryHandler : IRequestHandler<DownloadDocumentoQuery, Result<DownloadedFile>>
{
    private readonly IDocumentoRepository _documentoRepository;
    private readonly IFileStorageService _fileStorageService;

    public DownloadDocumentoQueryHandler(IDocumentoRepository documentoRepository, IFileStorageService fileStorageService)
    {
        _documentoRepository = documentoRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result<DownloadedFile>> Handle(DownloadDocumentoQuery request, CancellationToken cancellationToken)
    {
        // Placeholder for MVP
        await Task.CompletedTask;
        var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes("Conteúdo do arquivo de teste."));
        var downloadedFile = new DownloadedFile(stream, "text/plain", "teste.txt");
        return Result.Ok(downloadedFile);
    }
}

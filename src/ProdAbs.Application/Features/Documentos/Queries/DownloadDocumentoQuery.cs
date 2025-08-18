using MediatR;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Documentos.Queries;

public record DownloadedFile(Stream Content, string ContentType, string FileName);

public class DownloadDocumentoQuery : IRequest<Result<DownloadedFile>>
{
    public Guid Id { get; set; }
}

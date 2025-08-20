using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Documentos.Queries;

public class DownloadDocumentoQuery : IRequest<Result<FileDownloadDTO>>
{
    public Guid Id { get; set; }
}
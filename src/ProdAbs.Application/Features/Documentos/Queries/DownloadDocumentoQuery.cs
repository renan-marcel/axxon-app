using MediatR;
using ProdAbs.SharedKernel;
using System;
using System.IO;

namespace ProdAbs.Application.Features.Documentos.Queries
{
    public class DownloadDocumentoQuery : IRequest<Result<Stream>>
    {
        public Guid Id { get; set; }
    }
}
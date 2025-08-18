using MediatR;
using ProdAbs.SharedKernel;
using ProdAbs.Application.DTOs;
using System;

namespace ProdAbs.Application.Features.Documentos.Queries
{
    public class GetDocumentoByIdQuery : IRequest<Result<DocumentoDTO>>
    {
        public Guid Id { get; set; }
    }
}
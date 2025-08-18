using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Documentos.Queries;

public class GetDocumentoByIdQuery : IRequest<Result<DocumentoDTO>>
{
    public Guid Id { get; set; }
}

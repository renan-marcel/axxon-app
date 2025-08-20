using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Queries;

public class GetDocumentosDoProntuarioQuery : IRequest<Result<List<DocumentoDTO>>>
{
    public Guid ProntuarioId { get; set; }
}
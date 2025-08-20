using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.TiposDocumento.Queries;

public class ListarTiposDocumentoQuery : IRequest<Result<List<TipoDocumentoDetalhesDTO>>>
{
}
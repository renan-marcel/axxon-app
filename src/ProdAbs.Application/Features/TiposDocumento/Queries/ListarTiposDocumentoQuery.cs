using MediatR;
using ProdAbs.SharedKernel;
using ProdAbs.Application.DTOs;
using System.Collections.Generic;

namespace ProdAbs.Application.Features.TiposDocumento.Queries
{
    public class ListarTiposDocumentoQuery : IRequest<Result<List<TipoDocumentoDetalhesDTO>>>
    {
    }
}
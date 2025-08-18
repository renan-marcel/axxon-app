using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.TiposDocumento.Queries;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.TiposDocumento.Handlers
{
    public class ListarTiposDocumentoQueryHandler : IRequestHandler<ListarTiposDocumentoQuery, Result<List<TipoDocumentoDetalhesDTO>>>
    {
        private readonly ITipoDeDocumentoRepository _tipoDeDocumentoRepository;

        public ListarTiposDocumentoQueryHandler(ITipoDeDocumentoRepository tipoDeDocumentoRepository)
        {
            _tipoDeDocumentoRepository = tipoDeDocumentoRepository;
        }

        public async Task<Result<List<TipoDocumentoDetalhesDTO>>> Handle(ListarTiposDocumentoQuery request, CancellationToken cancellationToken)
        {
            var tiposDocumento = await _tipoDeDocumentoRepository.ListAsync();

            var dtos = tiposDocumento.Select(td => new TipoDocumentoDetalhesDTO
            {
                Id = td.Id,
                Nome = td.Nome,
                Campos = td.Campos
            }).ToList();

            return Result.Ok(dtos);
        }
    }
}
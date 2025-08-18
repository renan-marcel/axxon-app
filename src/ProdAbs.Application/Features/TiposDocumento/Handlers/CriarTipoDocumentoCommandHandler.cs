using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.Application.Features.TiposDocumento.Commands;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.TiposDocumento.Handlers
{
    public class CriarTipoDocumentoCommandHandler : IRequestHandler<CriarTipoDocumentoCommand, Result<TipoDocumentoDetalhesDTO>>
    {
        private readonly ITipoDeDocumentoRepository _repository;

        public CriarTipoDocumentoCommandHandler(ITipoDeDocumentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<TipoDocumentoDetalhesDTO>> Handle(CriarTipoDocumentoCommand request, CancellationToken cancellationToken)
        {
            var tipoDocumento = new TipoDocumento
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Campos = request.Campos.Select(c => new Domain.ValueObjects.CampoMetadata { Label = c }).ToList()
            };

            await _repository.AddAsync(tipoDocumento);

            var dto = new TipoDocumentoDetalhesDTO
            {
                Id = tipoDocumento.Id,
                Nome = tipoDocumento.Nome,
                Campos = tipoDocumento.Campos.Select(c => c.Label).ToList()
            };

            return Result.Ok(dto);
        }
    }
}

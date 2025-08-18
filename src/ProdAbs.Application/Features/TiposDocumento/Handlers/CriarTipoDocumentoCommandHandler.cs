using MediatR;
using ProdAbs.Application.Features.TiposDocumento.Commands;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.Interfaces;
using ProdAbs.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace ProdAbs.Application.Features.TiposDocumento.Handlers
{
    public class CriarTipoDocumentoCommandHandler : IRequestHandler<CriarTipoDocumentoCommand, Result<System.Guid>>
    {
        private readonly ITipoDeDocumentoRepository _tipoDeDocumentoRepository;

        public CriarTipoDocumentoCommandHandler(ITipoDeDocumentoRepository tipoDeDocumentoRepository)
        {
            _tipoDeDocumentoRepository = tipoDeDocumentoRepository;
        }

        public async Task<Result<System.Guid>> Handle(CriarTipoDocumentoCommand request, CancellationToken cancellationToken)
        {
            var tipoDocumento = new TipoDocumento
            {
                Nome = request.Nome,
                Campos = request.Campos
            };

            await _tipoDeDocumentoRepository.AddAsync(tipoDocumento);

            return Result.Ok(tipoDocumento.Id);
        }
    }
}
using MediatR;
using ProdAbs.SharedKernel;
using ProdAbs.Domain.ValueObjects;
using System.Collections.Generic;

namespace ProdAbs.Application.Features.TiposDocumento.Commands
{
    public class CriarTipoDocumentoCommand : IRequest<Result<System.Guid>>
    {
        public string Nome { get; set; }
        public List<CampoMetadata> Campos { get; set; }
    }
}
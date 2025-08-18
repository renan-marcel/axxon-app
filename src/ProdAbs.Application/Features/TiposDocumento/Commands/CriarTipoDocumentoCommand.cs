using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.SharedKernel;
using System.Collections.Generic;

namespace ProdAbs.Application.Features.TiposDocumento.Commands
{
    public class CriarTipoDocumentoCommand : IRequest<Result<TipoDocumentoDetalhesDTO>>
    {
        public string Nome { get; set; }
        public List<string> Campos { get; set; } = new();
    }
}

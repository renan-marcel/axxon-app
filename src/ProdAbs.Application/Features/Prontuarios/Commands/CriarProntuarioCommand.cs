using MediatR;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Commands
{
    public class CriarProntuarioCommand : IRequest<Result<System.Guid>>
    {
        public string IdentificadorEntidade { get; set; }
        public string TipoProntuario { get; set; }
    }
}
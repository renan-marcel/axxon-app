using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Commands;

public class CriarProntuarioCommand : IRequest<Result<ProntuarioResumoDTO>>
{
    public string IdentificadorEntidade { get; set; }
    public string TipoProntuario { get; set; }
}

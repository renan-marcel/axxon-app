using MediatR;
using ProdAbs.SharedKernel;

namespace ProdAbs.Application.Features.Prontuarios.Commands;

public class AdicionarDocumentoAoProntuarioCommand : IRequest<Result>
{
    public Guid ProntuarioId { get; set; }
    public Guid DocumentoId { get; set; }
}

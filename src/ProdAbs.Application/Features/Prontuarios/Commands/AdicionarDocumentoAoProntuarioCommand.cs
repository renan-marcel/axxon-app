using MediatR;
using ProdAbs.SharedKernel;
using System;

namespace ProdAbs.Application.Features.Prontuarios.Commands;

public class AdicionarDocumentoAoProntuarioCommand : IRequest<Result>
{
    public Guid ProntuarioId { get; set; }
    public Guid DocumentoId { get; set; }
}

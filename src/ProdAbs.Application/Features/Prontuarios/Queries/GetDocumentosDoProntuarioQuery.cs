using MediatR;
using ProdAbs.SharedKernel;
using ProdAbs.Application.DTOs;
using System;
using System.Collections.Generic;

namespace ProdAbs.Application.Features.Prontuarios.Queries
{
    public class GetDocumentosDoProntuarioQuery : IRequest<Result<List<DocumentoDTO>>>
    {
        public Guid ProntuarioId { get; set; }
    }
}
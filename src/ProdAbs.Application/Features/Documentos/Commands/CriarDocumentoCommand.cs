using MediatR;
using Microsoft.AspNetCore.Http;
using ProdAbs.SharedKernel;
using System;
using System.Collections.Generic;

namespace ProdAbs.Application.Features.Documentos.Commands
{
    public class CriarDocumentoCommand : IRequest<Result<Guid>>
    {
        public IFormFile File { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public Dictionary<string, string> DicionarioDeCamposValores { get; set; }
    }
}
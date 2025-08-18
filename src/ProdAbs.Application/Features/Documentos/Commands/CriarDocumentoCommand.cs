using MediatR;
using ProdAbs.SharedKernel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProdAbs.Application.Features.Documentos.Commands
{
    public class CriarDocumentoCommand : IRequest<Result<Guid>>
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public Dictionary<string, string> DicionarioDeCamposValores { get; set; }
    }
}
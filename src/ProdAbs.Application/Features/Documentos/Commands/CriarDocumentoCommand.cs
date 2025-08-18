using MediatR;
using ProdAbs.Application.DTOs;
using ProdAbs.SharedKernel;
using System.IO;
using System.Collections.Generic;

namespace ProdAbs.Application.Features.Documentos.Commands;

public class CriarDocumentoCommand : IRequest<Result<DocumentoDTO>>
{
    public Stream FileStream { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Guid TipoDocumentoId { get; set; }
    public Dictionary<string, string> DicionarioDeCamposValores { get; set; }
}


using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProdAbs.Application.Features.Documentos.Commands;
using ProdAbs.Application.Features.Documentos.Queries;
using ProdAbs.SharedKernel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProdAbs.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocumento([FromForm] IFormFile file, [FromForm] Guid tipoDocumentoId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Arquivo não enviado.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var command = new CriarDocumentoCommand
                {
                    FileStream = stream,
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    TipoDocumentoId = tipoDocumentoId
                };
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumento(Guid id)
        {
            var query = new GetDocumentoByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDocumento(Guid id)
        {
            var query = new DownloadDocumentoQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            // Assuming the stream returned by DownloadDocumentoQuery is ready to be read
            // and you have the original file name and content type from the DocumentoDTO
            // For MVP, we might need to retrieve these from the DTO or pass them along.
            // For now, just return the stream as a FileStreamResult.
            // This part needs refinement in Phase 5 when DTOs are fully defined.
            return File(result.Value, "application/octet-stream", "download"); // Placeholder content type and file name
        }
    }
}

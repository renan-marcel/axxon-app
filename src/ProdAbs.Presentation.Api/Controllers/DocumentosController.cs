using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProdAbs.Application.Features.Documentos.Commands;
using ProdAbs.Application.Features.Documentos.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdAbs.Presentation.Api.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] Guid tipoDocumentoId, [FromForm] Dictionary<string, string> metadados)
        {
            var command = new CriarDocumentoCommand
            {
                FileStream = file.OpenReadStream(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                TipoDocumentoId = tipoDocumentoId,
                DicionarioDeCamposValores = metadados
            };

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetDocumentoByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            var query = new DownloadDocumentoQuery { Id = id };
            var result = await _mediator.Send(query);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }
            return File(result.Value.Content, result.Value.ContentType, result.Value.FileName);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdAbs.Application.Features.Prontuarios.Commands;
using ProdAbs.Application.Features.Prontuarios.Queries;
using System;
using System.Threading.Tasks;

namespace ProdAbs.Presentation.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProntuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProntuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarProntuarioCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return CreatedAtAction(nameof(Criar), new { id = result.Value.Id }, result.Value);
        }

        [HttpPost("{prontuarioId}/documentos")]
        public async Task<IActionResult> AdicionarDocumento(Guid prontuarioId, [FromBody] AdicionarDocumentoRequest request)
        {
            var command = new AdicionarDocumentoAoProntuarioCommand
            {
                ProntuarioId = prontuarioId,
                DocumentoId = request.DocumentoId
            };
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }

        [HttpGet("{prontuarioId}/documentos")]
        public async Task<IActionResult> GetDocumentos(Guid prontuarioId)
        {
            var query = new GetDocumentosDoProntuarioQuery { ProntuarioId = prontuarioId };
            var result = await _mediator.Send(query);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }

    public class AdicionarDocumentoRequest
    {
        public Guid DocumentoId { get; set; }
    }
}

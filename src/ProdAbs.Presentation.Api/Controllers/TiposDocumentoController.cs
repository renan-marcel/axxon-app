using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProdAbs.Application.Features.TiposDocumento.Commands;
using ProdAbs.Application.Features.TiposDocumento.Queries;

namespace ProdAbs.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TiposDocumentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TiposDocumentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarTipoDocumentoCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return CreatedAtAction(nameof(Criar), new { id = result.Value.Id }, result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var query = new ListarTiposDocumentoQuery();
            var result = await _mediator.Send(query);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}

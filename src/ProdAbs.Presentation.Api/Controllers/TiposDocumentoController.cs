using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdAbs.Application.Features.TiposDocumento.Commands;
using ProdAbs.Application.Features.TiposDocumento.Queries;
using ProdAbs.SharedKernel;
using System.Threading.Tasks;

namespace ProdAbs.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class TiposDocumentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TiposDocumentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarTipoDocumento([FromBody] CriarTipoDocumentoCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTiposDocumento()
        {
            var result = await _mediator.Send(new ListarTiposDocumentoQuery());
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

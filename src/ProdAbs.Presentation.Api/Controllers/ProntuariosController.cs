
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProdAbs.Application.Features.Prontuarios.Commands;
using ProdAbs.Application.Features.Prontuarios.Queries;
using ProdAbs.SharedKernel;
using System;
using System.Threading.Tasks;

namespace ProdAbs.Presentation.Api.Controllers
{
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
        public async Task<IActionResult> CriarProntuario([FromBody] CriarProntuarioCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("{prontuarioId}/documentos/{documentoId}")]
        public async Task<IActionResult> AdicionarDocumentoAoProntuario(Guid prontuarioId, Guid documentoId)
        {
            var command = new AdicionarDocumentoAoProntuarioCommand { ProntuarioId = prontuarioId, DocumentoId = documentoId };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{prontuarioId}/documentos")]
        public async Task<IActionResult> GetDocumentosDoProntuario(Guid prontuarioId)
        {
            var query = new GetDocumentosDoProntuarioQuery { ProntuarioId = prontuarioId };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

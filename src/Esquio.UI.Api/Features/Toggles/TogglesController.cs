using Esquio.UI.Api.Features.Toggles.Delete;
using Esquio.UI.Api.Features.Toggles.Details;
using Esquio.UI.Api.Features.Toggles.Reveal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles
{
    [Authorize]
    [ApiController]
    public class TogglesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TogglesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpGet]
        [Route("api/v1/toggle/{toggleId:int:min(1)}")]
        public async Task<IActionResult> Get([FromRoute]DetailsToggleRequest detailsToggleRequest, CancellationToken cancellationToken = default)
        {
            var toggle = await _mediator.Send(detailsToggleRequest, cancellationToken);

            if (toggle != null)
            {
                return Ok(toggle);
            }

            return NotFound();
        }
        [HttpDelete]
        [Route("api/v1/toggle/{toggleId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteToggleRequest detailsToggleRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(detailsToggleRequest, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        [Route("api/v1/toggle/{toggleId:int:min(1)}/parameters/reveal")]
        public async Task<IActionResult> Get([FromRoute]RevealToggleRequest revealToggleRequest, CancellationToken cancellationToken = default)
        {
            var reveal = await _mediator.Send(revealToggleRequest, cancellationToken);

            return Ok(reveal);
        }
    }
}

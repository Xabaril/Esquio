using Esquio.UI.Api.Features.Toggles.Delete;
using Esquio.UI.Api.Features.Toggles.Details;
using Esquio.UI.Api.Features.Toggles.Known;
using Esquio.UI.Api.Features.Toggles.Parameter;
using Esquio.UI.Api.Features.Toggles.Post;
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
        [ActionName("Get")]
        public async Task<IActionResult> Get([FromRoute]DetailsToggleRequest detailsToggleRequest, CancellationToken cancellationToken = default)
        {
            var toggle = await _mediator.Send(detailsToggleRequest, cancellationToken);

            if (toggle != null)
            {
                return Ok(toggle);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("api/v1/toggle/{toggleId:int:min(1)}/parameters/reveal")]
        public async Task<IActionResult> Reveal([FromRoute]RevealToggleRequest revealToggleRequest, CancellationToken cancellationToken = default)
        {
            var reveal = await _mediator.Send(revealToggleRequest, cancellationToken);

            return Ok(reveal);
        }

        [HttpGet]
        [Route("api/v1/toggle/knowntypes")]
        public async Task<IActionResult> KnownTypes(CancellationToken cancellationToken = default)
        {
            var toggleList = await _mediator.Send(new KnownToggleRequest(), cancellationToken);

            return Ok(toggleList);
        }

        [HttpPost]
        [Route("api/v1/toggle")]
        public async Task<IActionResult> Post(PostToggleRequest postToggleRequest, CancellationToken cancellationToken = default)
        {
            var toggleId = await _mediator.Send(postToggleRequest, cancellationToken);
            //TODO:review location uri creation
            return Created($"api/v1/toggle/{toggleId}", null);
        }

        [HttpPost]
        [Route("api/v1/toggle/parameter")]
        public async Task<IActionResult> PostParameter(ParameterToggleRequest parameterToggleRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(parameterToggleRequest, cancellationToken);

            return Created($"api/v1/toggle/{parameterToggleRequest.ToogleId}", null);
        }

        [HttpDelete]
        [Route("api/v1/toggle/{toggleId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteToggleRequest detailsToggleRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(detailsToggleRequest, cancellationToken);

            return NoContent();
        }
    }
}

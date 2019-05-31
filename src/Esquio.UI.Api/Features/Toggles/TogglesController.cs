using Esquio.UI.Api.Features.Toggles.Add;
using Esquio.UI.Api.Features.Toggles.AddParameter;
using Esquio.UI.Api.Features.Toggles.Delete;
using Esquio.UI.Api.Features.Toggles.Details;
using Esquio.UI.Api.Features.Toggles.KnownTypes;
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
        [Route("api/v1/toggles/{toggleId:int:min(1)}")]
        public async Task<IActionResult> Details([FromRoute]DetailsToggleRequest detailsToggleRequest, CancellationToken cancellationToken = default)
        {
            var toggle = await _mediator.Send(detailsToggleRequest, cancellationToken);

            if (toggle != null)
            {
                return Ok(toggle);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("api/v1/toggles/{toggleId:int:min(1)}/parameters")]
        public async Task<IActionResult> AddParameter(int toggleId, AddParameterToggleRequest parameterToggleRequest, CancellationToken cancellationToken = default)
        {
            parameterToggleRequest.ToggleId = toggleId;
            await _mediator.Send(parameterToggleRequest, cancellationToken);

            return Created($"api/v1/toggle/{parameterToggleRequest.ToggleId}", null);
        }

        [HttpDelete]
        [Route("api/v1/toggles/{toggleId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteToggleRequest detailsToggleRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(detailsToggleRequest, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        [Route("api/v1/toggles/parameters/{toggleType}")]
        public async Task<IActionResult> Reveal([FromRoute]RevealToggleRequest revealToggleRequest, CancellationToken cancellationToken = default)
        {
            var reveal = await _mediator.Send(revealToggleRequest, cancellationToken);

            return Ok(reveal);
        }

        [HttpGet]
        [Route("api/v1/toggles/types")]
        public async Task<IActionResult> KnownTypes(CancellationToken cancellationToken = default)
        {
            var toggleList = await _mediator.Send(new KnownTypesToggleRequest(), cancellationToken);

            return Ok(toggleList);
        }

        [HttpPost]
        [Route("api/v1/toggles")]
        public async Task<IActionResult> Add(AddToggleRequest postToggleRequest, CancellationToken cancellationToken = default)
        {
            var toggleId = await _mediator.Send(postToggleRequest, cancellationToken);

            return Created($"api/v1/toggle/{toggleId}", null);
        }
    }
}

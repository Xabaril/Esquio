using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Shared.Models.Toggles.Add;
using Esquio.UI.Api.Shared.Models.Toggles.AddParameter;
using Esquio.UI.Api.Shared.Models.Toggles.Delete;
using Esquio.UI.Api.Shared.Models.Toggles.Details;
using Esquio.UI.Api.Shared.Models.Toggles.KnownTypes;
using Esquio.UI.Api.Shared.Models.Toggles.Reveal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Toggles
{
    [Authorize]
    [ApiVersion("5.0")]
    [ApiController]
    public class TogglesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TogglesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Authorize(Policies.Reader)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/toggles/{toggleType}")]
        [ProducesResponseType(typeof(DetailsToggleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DetailsToggleResponse>> Details(string productName, string featureName, string toggleType, CancellationToken cancellationToken = default)
        {
            var request = new DetailsToggleRequest()
            {
                ProductName = productName,
                FeatureName = featureName,
                ToggleType = toggleType
            };

            var toggle = await _mediator.Send(request, cancellationToken);

            if (toggle != null)
            {
                return Ok(toggle);
            }

            return NotFound();
        }

        [HttpDelete]
        [Authorize(Policies.Contributor)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/toggles/{toggleType}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string productName, string featureName, string toggleType, CancellationToken cancellationToken = default)
        {
            var request = new DeleteToggleRequest()
            {
                ProductName = productName,
                FeatureName = featureName,
                ToggleType = toggleType
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policies.Reader)]
        [Route("api/toggles/parameters/{toggleType}")]
        [ProducesResponseType(typeof(RevealToggleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RevealToggleResponse>> Reveal(string toggleType, CancellationToken cancellationToken = default)
        {
            var request = new RevealToggleRequest()
            {
                ToggleType = toggleType
            };

            var reveal = await _mediator.Send(request, cancellationToken);

            return Ok(reveal);
        }

        [HttpGet]
        [Authorize(Policies.Reader)]
        [Route("api/toggles/types")]
        [ProducesResponseType(typeof(KnownTypesToggleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KnownTypesToggleResponse>> KnownTypes(CancellationToken cancellationToken = default)
        {
            var toggleList = await _mediator.Send(new KnownTypesToggleRequest(), cancellationToken);

            return Ok(toggleList);
        }

        [HttpPost]
        [Authorize(Policies.Contributor)]
        [Route("api/toggles/parameters")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddParameter(AddParameterToggleRequest parameterToggleRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(parameterToggleRequest, cancellationToken);

            //TODO: fix created at action
            return Created($"api/v1/products/{parameterToggleRequest.ProductName}/features/{parameterToggleRequest.FeatureName}/toggles/{parameterToggleRequest.ToggleType}", null);
        }

        [HttpPost]
        [Authorize(Policies.Contributor)]
        [Route("api/toggles")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(AddToggleRequest postToggleRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(postToggleRequest, cancellationToken);

            return Created($"api/v1/products/{postToggleRequest.ProductName}/features/{postToggleRequest.FeatureName}/toggles/{postToggleRequest.ToggleType}", null);
        }
    }
}

using Esquio.UI.Api.Features.Flags.Add;
using Esquio.UI.Api.Features.Flags.Delete;
using Esquio.UI.Api.Features.Flags.Details;
using Esquio.UI.Api.Features.Flags.List;
using Esquio.UI.Api.Features.Flags.Rollback;
using Esquio.UI.Api.Features.Flags.Rollout;
using Esquio.UI.Api.Features.Flags.Update;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags
{
    [Authorize]
    [ApiVersion("2.0")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FeaturesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug}/features")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(string productName,AddFeatureRequest addfeatureRequest, CancellationToken cancellationToken = default)
        {
            addfeatureRequest.ProductName = productName;

            await _mediator.Send(addfeatureRequest, cancellationToken);

            return Created($"api/v1/products/{addfeatureRequest.ProductName}/features/{addfeatureRequest.Name}", null);
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string productName, string featureName, UpdateFeatureRequest updateFeatureRequest, CancellationToken cancellationToken = default)
        {
            updateFeatureRequest.CurrentName = featureName;
            updateFeatureRequest.ProductName = productName;

            await _mediator.Send(updateFeatureRequest, cancellationToken);

            Response.Headers.Add(HeaderNames.Location, $"api/v1/products/{updateFeatureRequest.ProductName}/features/{updateFeatureRequest.Name}");
            return Ok();
        }


        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}/rollout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Rollout([FromRoute]RolloutFeatureRequest rolloutFeatureRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(rolloutFeatureRequest, cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}/rollback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Rollback([FromRoute]RollbackFlagRequest rollbackFeatureRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(rollbackFeatureRequest, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute]DeleteFeatureRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policies.Read)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}")]
        [ProducesResponseType(typeof(DetailsFeatureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DetailsFeatureResponse>> Get(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            var feature = await _mediator.Send(new DetailsFeatureRequest(productName, featureName), cancellationToken);

            if (feature != null)
            {
                return Ok(feature);
            }

            return NotFound();
        }

        [HttpGet]
        [Authorize(Policies.Read)]
        [Route("api/products/{productName:slug}/features")]
        [ProducesResponseType(typeof(ListFeatureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListFeatureResponse>> List(string productName, [FromQuery]ListFeatureRequest request, CancellationToken cancellationToken = default)
        {
            request.ProductName = productName;

            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }
    }
}

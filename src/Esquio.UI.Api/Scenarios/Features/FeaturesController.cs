using Esquio.UI.Api.Scenarios.Flags.Add;
using Esquio.UI.Api.Scenarios.Flags.Archive;
using Esquio.UI.Api.Scenarios.Flags.Delete;
using Esquio.UI.Api.Scenarios.Flags.Details;
using Esquio.UI.Api.Scenarios.Flags.List;
using Esquio.UI.Api.Scenarios.Flags.Rollback;
using Esquio.UI.Api.Scenarios.Flags.Rollout;
using Esquio.UI.Api.Scenarios.Flags.Update;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags
{
    [Authorize]
    [ApiVersion("3.0")]
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
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(string productName, AddFeatureRequest addfeatureRequest, CancellationToken cancellationToken = default)
        {
            addfeatureRequest.ProductName = productName;

            await _mediator.Send(addfeatureRequest, cancellationToken);

            return Created($"api/v1/products/{addfeatureRequest.ProductName}/features/{addfeatureRequest.Name}", null);
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string productName, string featureName, UpdateFeatureRequest updateFeatureRequest, CancellationToken cancellationToken = default)
        {
            updateFeatureRequest.CurrentName = featureName;
            updateFeatureRequest.ProductName = productName;

            await _mediator.Send(updateFeatureRequest, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/rollout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Rollout(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            var request = new RolloutFeatureRequest()
            {
                ProductName = productName,
                FeatureName = featureName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/rollback")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Rollback(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            var request = new RollbackFeatureRequest()
            {
                ProductName = productName,
                FeatureName = featureName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/archive")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Archive(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            var request = new ArchiveFeatureRequest()
            {
                ProductName = productName,
                FeatureName = featureName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            var request = new DeleteFeatureRequest()
            {
                ProductName = productName,
                FeatureName = featureName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policies.Read)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}")]
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
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features")]
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

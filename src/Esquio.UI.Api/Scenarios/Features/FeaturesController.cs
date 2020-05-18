using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Features.Add;
using Esquio.UI.Api.Shared.Models.Features.Archive;
using Esquio.UI.Api.Shared.Models.Features.Delete;
using Esquio.UI.Api.Shared.Models.Features.Details;
using Esquio.UI.Api.Shared.Models.Features.List;
using Esquio.UI.Api.Shared.Models.Features.Rollback;
using Esquio.UI.Api.Shared.Models.Features.Rollout;
using Esquio.UI.Api.Shared.Models.Features.State;
using Esquio.UI.Api.Shared.Models.Features.Update;
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
        [Authorize(Policies.Contributor)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(string productName, AddFeatureRequest addfeatureRequest, CancellationToken cancellationToken = default)
        {
            addfeatureRequest.ProductName = productName;

            await _mediator.Send(addfeatureRequest, cancellationToken);

            return Created($"api/v1/products/{addfeatureRequest.ProductName}/features/{addfeatureRequest.Name}", null);
        }

        [HttpPut]
        [Authorize(Policies.Contributor)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string productName, string featureName, UpdateFeatureRequest updateFeatureRequest, CancellationToken cancellationToken = default)
        {
            updateFeatureRequest.CurrentName = featureName;
            updateFeatureRequest.ProductName = productName;

            await _mediator.Send(updateFeatureRequest, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        [Authorize(Policies.Contributor)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/deployments/{deploymentName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/rollout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Rollout(string productName,string deploymentName, string featureName, CancellationToken cancellationToken = default)
        {
            var request = new RolloutFeatureRequest()
            {
                ProductName = productName,
                DeploymentName = deploymentName,
                FeatureName = featureName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        [Authorize(Policies.Contributor)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/deployments/{deploymentName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/rollback")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Rollback(string productName, string deploymentName, string featureName, CancellationToken cancellationToken = default)
        {
            var request = new RollbackFeatureRequest()
            {
                ProductName = productName,
                DeploymentName = deploymentName,
                FeatureName = featureName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        [Authorize(Policies.Contributor)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/archive")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policies.Contributor)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policies.Reader)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(typeof(DetailsFeatureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [Authorize(Policies.Reader)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/state")]
        [ProducesResponseType(typeof(StateFeatureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StateFeatureResponse>> GetState(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            var feature = await _mediator.Send(new StateFeatureRequest(productName, featureName), cancellationToken);

            if (feature != null)
            {
                return Ok(feature);
            }

            return NotFound();
        }

        [HttpGet]
        [Authorize(Policies.Reader)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features")]
        [ProducesResponseType(typeof(PaginatedResult<ListFeatureResponseDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResult<ListFeatureResponseDetail>>> List(string productName, [FromQuery]ListFeatureRequest request, CancellationToken cancellationToken = default)
        {
            request.ProductName = productName;

            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }
    }
}

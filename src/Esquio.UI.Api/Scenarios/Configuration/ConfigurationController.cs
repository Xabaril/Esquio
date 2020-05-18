using Esquio.UI.Api.Shared.Models.Configuration.Details;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Store
{
    [Authorize]
    [ApiVersion("3.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController
         : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConfigurationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("product/{productName}/feature/{featureName}")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(DetailsConfigurationResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<DetailsConfigurationResponse>> Get([FromRoute]DetailsConfigurationRequest request, [FromQuery] string deployment, CancellationToken cancellationToken = default)
        {
            request.DeploymentName = deployment;

            var feature = await _mediator.Send(request, cancellationToken);

            if (feature != null)
            {
                return Ok(feature);
            }

            return NotFound();
        }
    }
}

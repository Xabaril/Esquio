using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Scenarios.Store.Details;
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
    public class StoreController
         : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoreController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("product/{productName}/feature/{featureName}")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(DetailsStoreResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<DetailsStoreResponse>> Get([FromRoute]DetailsStoreRequest request, [FromQuery] string ring, CancellationToken cancellationToken = default)
        {
            request.RingName = ring;

            var feature = await _mediator
                .Send(request, cancellationToken);

            return Ok(feature);
        }
    }
}

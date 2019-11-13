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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags
{
    [Authorize]
    [ApiController]
    public class FlagsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FlagsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Authorize(Policies.Write)]
        [Route("api/v1/flags")]
        public async Task<IActionResult> Add(AddFlagRequest addFlagRequest, CancellationToken cancellationToken = default)
        {
            var flagId = await _mediator.Send(addFlagRequest, cancellationToken);

            return Created($"api/v1/flags/{flagId}", null);
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/v1/flags")]
        public async Task<IActionResult> Update(UpdateFlagRequest updateFlagRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(updateFlagRequest, cancellationToken);

            return Ok();
        }


        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/v1/flags/{featureId:int:min(1)}/rollout")]
        public async Task<IActionResult> Rollout([FromRoute]RolloutFlagRequest rolloutFlagRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(rolloutFlagRequest, cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policies.Write)]
        [Route("api/v1/products/{productName}/flags/{featureName}/rollback")]
        public async Task<IActionResult> Rollback([FromRoute]RollbackFlagRequest rollbackFlagRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(rollbackFlagRequest, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Policies.Write)]
        [Route("api/v1/products/{productName}/flags/{featureName}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteFlagRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policies.Read)]
        [Route("api/v1/products/{productName}/flags/{featureName}")]
        public async Task<IActionResult> Get([FromRoute]DetailsFlagRequest request, CancellationToken cancellationToken = default)
        {
            var feature = await _mediator.Send(request, cancellationToken);

            if (feature != null)
            {
                return Ok(feature);
            }

            return NotFound();
        }

        [HttpGet]
        [Authorize(Policies.Read)]
        [Route("api/v1/products/{productName}/flags")]
        public async Task<IActionResult> List(string productName, [FromQuery]ListFlagRequest request, CancellationToken cancellationToken = default)
        {
            request.ProductName = productName;

            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }
    }
}

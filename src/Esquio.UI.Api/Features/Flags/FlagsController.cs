using Esquio.UI.Api.Features.Flags.Delete;
using Esquio.UI.Api.Features.Flags.Rollout;
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

        [HttpPut]
        [Route("api/v1/product/{productId:int:min(1)}/flags/{featureId:int:min(1)}/rollout")]
        public async Task<IActionResult> Rollout([FromRoute]RolloutFlagRequest rolloutFlagRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(rolloutFlagRequest, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/product/{productId:int:min(1)}/flags/{featureId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteFlagRequest request)
        {
            await _mediator.Send(request);

            return NoContent();
        }

        //[HttpGet]
        //[Route("api/v1/product/{id:int}/flags/{id:int}")]
        //public IActionResult GetBy(DetailsFlagRequest request, CancellationToken cancellationToken = default)
        //{
        //    var flag = _mediator.Send(request, cancellationToken);

        //    return Ok(flag);
        //}


    }
}

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
        [Route("api/v1/product/{ProductId:int:min(1)}/flags/{FeatureId:int:min(1)}/rollout")]
        public async Task<IActionResult> Rollout([FromRoute]RolloutFlagRequest rolloutFlagRequest, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(rolloutFlagRequest, cancellationToken);

            return Ok();
        }


        //[HttpGet]
        //[Route("api/v1/product/{id:int}/flags/{id:int}")]
        //public IActionResult GetBy(int productId, int flagId)
        //{
        //    return Ok();
        //}

        //[HttpGet]
        //[Route("api/v1/product/{id:int}/flags")]
        //public IActionResult GetBy(int productId)
        //{
        //    return Ok();
        //}

        //[HttpPost]
        //[Route("api/v1/product/{id:int}/flags")]
        //public IActionResult Add(int productId)
        //{
        //    return Created(string.Empty, null);
        //}

        //[HttpPut]
        //[Route("api/v1/features")]
        //public IActionResult Update()
        //{
        //    return Ok();
        //}

        

        //[HttpDelete]
        //[Route("api/v1/features/{id:int}")]
        //public IActionResult Delete(int id)
        //{
        //    return NoContent();
        //}
    }
}

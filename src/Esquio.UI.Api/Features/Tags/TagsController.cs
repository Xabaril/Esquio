using Esquio.UI.Api.Features.Flags.Delete;
using Esquio.UI.Api.Features.Flags.Details;
using Esquio.UI.Api.Features.Flags.List;
using Esquio.UI.Api.Features.Flags.Rollout;
using Esquio.UI.Api.Features.Tags.Add;
using Esquio.UI.Api.Features.Tags.Delete;
using Esquio.UI.Api.Features.Tags.List;
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
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TagsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("api/v1/tags/{featureId:int:min(1)}")]
        public async Task<IActionResult> List([FromRoute]ListTagRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        [HttpDelete]
        [Route("api/v1/tags/{featureId:int:min(1)}/{tag}")]
        public async Task<IActionResult> Untag([FromRoute]DeleteTagRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPost]
        [Route("api/v1/tags/{featureId:int:min(1)}")]
        public async Task<IActionResult> Tag(int featureId, AddTagRequest request, CancellationToken cancellationToken = default)
        {
            request.FeatureId = featureId;
            await _mediator.Send(request, cancellationToken);

            return Ok();
        }
    }
}

using Esquio.UI.Api.Features.Flags.Delete;
using Esquio.UI.Api.Features.Flags.Details;
using Esquio.UI.Api.Features.Flags.List;
using Esquio.UI.Api.Features.Flags.Rollout;
using Esquio.UI.Api.Features.Tags.Delete;
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

        [HttpDelete]
        [Route("api/v1/tags/{tag}/flags/{featureId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteTagRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}

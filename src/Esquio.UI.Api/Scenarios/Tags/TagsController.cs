using Esquio.UI.Api.Features.Tags.Add;
using Esquio.UI.Api.Features.Tags.Delete;
using Esquio.UI.Api.Features.Tags.List;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Tags
{
    [Authorize]
    [ApiVersion("2.0")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TagsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Authorize(Policies.Read)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}/tags")]
        public async Task<ActionResult<IEnumerable<TagResponseDetail>>> List([FromRoute]ListTagRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        [HttpDelete]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}/tags/untag/{tag:slug}")]
        public async Task<IActionResult> Untag([FromRoute]DeleteTagRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug}/features/{featureName:slug}/tags/tag")]
        public async Task<IActionResult> Tag(string productName,string featureName, AddTagRequest request, CancellationToken cancellationToken = default)
        {
            request.ProductName = productName;
            request.FeatureName = featureName;

            await _mediator.Send(request, cancellationToken);

            return Ok();
        }
    }
}

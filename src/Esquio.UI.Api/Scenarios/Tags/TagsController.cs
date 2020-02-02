using Esquio.UI.Api.Scenarios.Tags.Add;
using Esquio.UI.Api.Scenarios.Tags.Delete;
using Esquio.UI.Api.Scenarios.Tags.List;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Tags
{
    [Authorize]
    [ApiVersion("3.0")]
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
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/tags")]
        [ProducesResponseType(typeof(List<TagResponseDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TagResponseDetail>>> List(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            var request = new ListTagRequest()
            {
                ProductName = productName,
                FeatureName = featureName
            };

            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        [HttpDelete]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/tags/untag/{tag:slug}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Untag(string productName, string featureName, string tag, CancellationToken cancellationToken = default)
        {
            var request = new DeleteTagRequest()
            {
                ProductName = productName,
                FeatureName = featureName,
                Tag = tag
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Policies.Write)]
        [Route("api/products/{productName:slug:minlength(5):maxlength(200)}/features/{featureName:slug:minlength(5):maxlength(200)}/tags/tag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Tag(string productName, string featureName, AddTagRequest request, CancellationToken cancellationToken = default)
        {
            request.ProductName = productName;
            request.FeatureName = featureName;

            await _mediator.Send(request, cancellationToken);

            //TODO: fix created at action and set NoContent
            return Ok();
        }
    }
}

using Esquio.UI.Api.Scenarios.ApiKeys.Add;
using Esquio.UI.Api.Scenarios.ApiKeys.Details;
using Esquio.UI.Api.Scenarios.ApiKeys.List;
using Esquio.UI.Api.Scenarios.Flags.Delete;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.ApiKeys
{
    [Authorize()]
    [ApiVersion("3.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiKeysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiKeysController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("")]
        [ProducesResponseType(typeof(ListApiKeyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListApiKeyResponse>> List([FromQuery]ListApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            var apiKeys = await _mediator.Send(request, cancellationToken);

            return Ok(apiKeys);
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("{name:slug}")]
        [ProducesResponseType(typeof(DetailsApiKeyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DetailsApiKeyResponse>> Get(string name, CancellationToken cancellationToken = default)
        {
            var apiKeys = await _mediator.Send(new DetailsApiKeyRequest { Name = name }, cancellationToken);

            if (apiKeys != null)
            {
                return Ok(apiKeys);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Management)]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddApiKeyResponse>> Add(AddApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return Created($"api/apikeys/{request.Name}?api-version=2.0", response);
        }

        [HttpDelete]
        [Authorize(Policies.Management)]
        [Route("{name:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string name, CancellationToken cancellationToken = default)
        {
            var request = new DeleteApiKeyRequest()
            {
                Name = name
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}

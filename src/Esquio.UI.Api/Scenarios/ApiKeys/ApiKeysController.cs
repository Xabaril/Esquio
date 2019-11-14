using Esquio.UI.Api.Features.ApiKeys.Add;
using Esquio.UI.Api.Features.ApiKeys.Details;
using Esquio.UI.Api.Features.ApiKeys.List;
using Esquio.UI.Api.Features.Flags.Delete;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.ApiKeys
{
    [Authorize()]
    [ApiVersion("2.0")]
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
        public async Task<IActionResult> List([FromQuery]ListApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            var apiKeys = await _mediator.Send(request, cancellationToken);

            return Ok(apiKeys);
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("{apiKeyId:int:min(1)}")]
        public async Task<IActionResult> Get([FromRoute]DetailsApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            var apiKeys = await _mediator.Send(request, cancellationToken);

            if (apiKeys != null)
            {
                return Ok(apiKeys);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Management)]
        [Route("")]
        public async Task<IActionResult> Add(AddApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return Created($"api/v1/apikeys/{response.ApiKeyId}", response);
        }

        [HttpDelete]
        [Authorize(Policies.Management)]
        [Route("{apiKeyId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}

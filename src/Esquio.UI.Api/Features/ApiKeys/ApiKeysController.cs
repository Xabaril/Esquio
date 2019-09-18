using Esquio.UI.Api.Features.ApiKeys.Add;
using Esquio.UI.Api.Features.ApiKeys.Details;
using Esquio.UI.Api.Features.ApiKeys.List;
using Esquio.UI.Api.Features.Flags.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.ApiKeys
{
    [Authorize()]
    [ApiController]
    public class ApiKeysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiKeysController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("api/v1/apikeys")]
        public async Task<IActionResult> List([FromQuery]ListApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            var apiKeys = await _mediator.Send(request, cancellationToken);

            return Ok(apiKeys);
        }

        [HttpGet]
        [Route("api/v1/apikeys/{apiKeyId:int:min(1)}")]
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
        [Route("api/v1/apikeys")]
        public async Task<IActionResult> Add(AddApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return Created($"api/v1/apikeys/{response.ApiKeyId}", response);
        }

        [HttpDelete]
        [Route("api/v1/apikeys/{apiKeyId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteApiKeyRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}

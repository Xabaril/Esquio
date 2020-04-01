using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Audit.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Audit
{
    [Authorize]
    [ApiVersion("3.0")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuditController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("api/audit")]
        [ProducesResponseType(typeof(ListAuditRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResult<ListAuditResponseDetail>>> List([FromQuery]ListAuditRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }
    }
}

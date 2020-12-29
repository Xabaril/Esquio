using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Infrastructure.Services;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Permissions.Add;
using Esquio.UI.Api.Shared.Models.Permissions.Delete;
using Esquio.UI.Api.Shared.Models.Permissions.Details;
using Esquio.UI.Api.Shared.Models.Permissions.List;
using Esquio.UI.Api.Shared.Models.Permissions.My;
using Esquio.UI.Api.Shared.Models.Permissions.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Permissions
{
    [Authorize]
    [ApiVersion("5.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController
        : Controller
    {
        private readonly IMediator _mediator;
        private readonly ISubjectClaimsPrincipalFactory _subjectClaimsPrincipalFactory;

        public PermissionsController(IMediator mediator, ISubjectClaimsPrincipalFactory subjectClaimsPrincipalFactory)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _subjectClaimsPrincipalFactory = subjectClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(subjectClaimsPrincipalFactory));
        }

        [HttpGet]
        [Route("my")]
        [ProducesResponseType(typeof(MyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> My(CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new MyRequest()
            {
                SubjectId = _subjectClaimsPrincipalFactory.GetSubject(User)
            }, cancellationToken);

            if (response.IsAuthorized)
            {
                return Ok(response);
            }

            return Forbid();
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("")]
        [ProducesResponseType(typeof(PaginatedResult<ListUsersResponseDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResult<ListUsersResponseDetail>>> List([FromQuery]ListPermissionRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("{subjectId}")]
        [ProducesResponseType(typeof(DetailsPermissionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DetailsPermissionResponse>> Details(string subjectId, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(
                new DetailsPermissionRequest
                {
                    SubjectId = subjectId }
                , cancellationToken);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Policies.Management)]
        [Route("{subjectid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string subjectId, CancellationToken cancellationToken = default)
        {

            await _mediator.Send(new DeletePermissionRequest()
            {
                SubjectId = subjectId
            }, cancellationToken);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Policies.Management)]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody]AddPermissionRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);
            //TODO: fix url  and set NoContent
            return Ok();
        }

        [HttpPut]
        [Authorize(Policies.Management)]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody]UpdatePermissionRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}

using Esquio.UI.Api.Scenarios.Users.Add;
using Esquio.UI.Api.Scenarios.Users.Delete;
using Esquio.UI.Api.Scenarios.Users.Details;
using Esquio.UI.Api.Scenarios.Users.List;
using Esquio.UI.Api.Scenarios.Users.My;
using Esquio.UI.Api.Scenarios.Users.Update;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Users
{
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController
        : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("my")]
        [ProducesResponseType(typeof(MyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> My(CancellationToken cancellationToken = default)
        {
            var request = new MyRequest()
            {
                SubjectId = User.GetSubjectId()
            };

            var response = await _mediator.Send(request, cancellationToken);

            if (response.IsAuthorized)
            {
                return Ok(response);
            }

            return Forbid();
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("")]
        [ProducesResponseType(typeof(ListUsersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListUsersResponse>> List([FromQuery]ListUsersRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Policies.Management)]
        [Route("{subjectId}")]
        [ProducesResponseType(typeof(DetailsUsersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DetailsUsersResponse>> Details(string subjectId, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new DetailsUsersRequest { SubjectId = subjectId }, cancellationToken);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Policies.Management)]
        [Route("{subjectid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string subjectId, CancellationToken cancellationToken = default)
        {
            var request = new DeleteUsersRequest()
            {
                SubjectId = subjectId
            };

            await _mediator.Send(request, cancellationToken);
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

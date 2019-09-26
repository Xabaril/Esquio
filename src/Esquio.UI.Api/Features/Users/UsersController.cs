using Esquio.UI.Api.Features.Users.My;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Users
{
    [Authorize]
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
        [Route("api/v1/users/my")]
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
    }
}

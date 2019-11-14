using MediatR;

namespace Esquio.UI.Api.Features.Users.Details
{
    public class DetailsUsersRequest
        : IRequest<DetailsUsersResponse>
    {
        public string SubjectId { get; set; }
    }
}

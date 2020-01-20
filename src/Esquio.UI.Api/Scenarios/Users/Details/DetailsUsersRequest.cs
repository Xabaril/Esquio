using MediatR;

namespace Esquio.UI.Api.Scenarios.Users.Details
{
    public class DetailsUsersRequest
        : IRequest<DetailsUsersResponse>
    {
        public string SubjectId { get; set; }
    }
}

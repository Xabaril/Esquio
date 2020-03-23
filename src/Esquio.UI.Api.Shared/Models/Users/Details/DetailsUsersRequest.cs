using MediatR;

namespace Esquio.UI.Api.Shared.Models.Users.Details
{
    public class DetailsUsersRequest
        : IRequest<DetailsUsersResponse>
    {
        public string SubjectId { get; set; }
    }
}

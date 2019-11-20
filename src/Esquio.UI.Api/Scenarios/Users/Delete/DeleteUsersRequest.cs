using MediatR;

namespace Esquio.UI.Api.Features.Users.Delete
{
    public class DeleteUsersRequest
        :IRequest
    {
        public string SubjectId { get; set; }
    }
}

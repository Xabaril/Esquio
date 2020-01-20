using MediatR;

namespace Esquio.UI.Api.Scenarios.Users.Delete
{
    public class DeleteUsersRequest
        :IRequest
    {
        public string SubjectId { get; set; }
    }
}

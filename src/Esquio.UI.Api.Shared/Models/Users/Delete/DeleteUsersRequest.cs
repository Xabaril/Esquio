using MediatR;

namespace Esquio.UI.Api.Shared.Models.Users.Delete
{
    public class DeleteUsersRequest
        :IRequest
    {
        public string SubjectId { get; set; }
    }
}

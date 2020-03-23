using MediatR;

namespace Esquio.UI.Api.Shared.Models.Users.Add
{
    public class AddPermissionRequest
        : IRequest
    {
        public string SubjectId { get; set; }

        public string ActAs { get; set; }
    }
}

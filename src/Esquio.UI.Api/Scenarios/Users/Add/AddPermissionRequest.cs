using MediatR;

namespace Esquio.UI.Api.Scenarios.Users.Add
{
    public class AddPermissionRequest
        : IRequest
    {
        public string SubjectId { get; set; }

        public string ActAs { get; set; }
    }
}

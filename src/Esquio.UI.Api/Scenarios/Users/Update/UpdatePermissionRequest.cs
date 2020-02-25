using MediatR;

namespace Esquio.UI.Api.Scenarios.Users.Update
{
    public class UpdatePermissionRequest
        : IRequest
    {
        public string SubjectId { get; set; }

        public string ActAs { get; set; }
    }
}

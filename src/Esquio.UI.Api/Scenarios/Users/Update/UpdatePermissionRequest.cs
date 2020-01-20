using MediatR;

namespace Esquio.UI.Api.Scenarios.Users.Update
{
    public class UpdatePermissionRequest
        : IRequest
    {
        public string SubjectId { get; set; }

        public bool Read { get; set; }

        public bool Write { get; set; }

        public bool Manage { get; set; }
    }
}

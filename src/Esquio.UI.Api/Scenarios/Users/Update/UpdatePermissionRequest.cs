using MediatR;

namespace Esquio.UI.Api.Features.Users.Update
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

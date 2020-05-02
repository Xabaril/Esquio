using MediatR;

namespace Esquio.UI.Api.Shared.Models.Permissions.Update
{
    public class UpdatePermissionRequest
        : IRequest
    {
        public string SubjectId { get; set; }

        public string ActAs { get; set; }
    }
}

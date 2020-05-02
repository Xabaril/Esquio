using MediatR;

namespace Esquio.UI.Api.Shared.Models.Permissions.Add
{
    public class AddPermissionRequest
        : IRequest
    {
        public string SubjectId { get; set; }

        public string ActAs { get; set; }
    }
}

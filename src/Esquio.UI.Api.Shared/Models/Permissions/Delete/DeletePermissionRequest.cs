using MediatR;

namespace Esquio.UI.Api.Shared.Models.Permissions.Delete
{
    public class DeletePermissionRequest
        :IRequest
    {
        public string SubjectId { get; set; }
    }
}

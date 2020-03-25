using MediatR;

namespace Esquio.UI.Api.Shared.Models.Permissions.Details
{
    public class DetailsPermissionRequest
        : IRequest<DetailsPermissionResponse>
    {
        public string SubjectId { get; set; }
    }
}

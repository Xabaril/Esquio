using MediatR;

namespace Esquio.UI.Api.Features.Toggles.Delete
{
    public class DeleteToggleRequest : IRequest
    {
        public int ToggleId { get; set; }
    }
}

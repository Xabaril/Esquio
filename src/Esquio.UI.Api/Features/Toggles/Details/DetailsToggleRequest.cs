using MediatR;

namespace Esquio.UI.Api.Features.Toggles.Details
{
    public class DetailsToggleRequest : IRequest<DetailsToggleResponse>
    {
        public int ToggleId { get; set; }
    }
}

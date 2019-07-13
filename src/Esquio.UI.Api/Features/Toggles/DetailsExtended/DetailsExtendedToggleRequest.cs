using MediatR;

namespace Esquio.UI.Api.Features.Toggles.DetailsExtended
{
    public class DetailsExtendedToggleRequest : IRequest<DetailsExtendedToggleResponse>
    {
        public int ToggleId { get; set; }
    }
}

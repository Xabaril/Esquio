using MediatR;

namespace Esquio.UI.Api.Features.Toggles.Reveal
{
    public class RevealToggleRequest : IRequest<RevealToggleResponse>
    {
        public int ToggleId { get; set; }
    }
}

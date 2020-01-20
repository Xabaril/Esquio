using MediatR;

namespace Esquio.UI.Api.Scenarios.Toggles.Reveal
{
    public class RevealToggleRequest : IRequest<RevealToggleResponse>
    {
        public string  ToggleType { get; set; }
    }
}

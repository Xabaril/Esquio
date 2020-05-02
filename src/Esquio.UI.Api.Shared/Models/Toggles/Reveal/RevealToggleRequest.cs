using MediatR;

namespace Esquio.UI.Api.Shared.Models.Toggles.Reveal
{
    public class RevealToggleRequest : IRequest<RevealToggleResponse>
    {
        public string  ToggleType { get; set; }
    }
}

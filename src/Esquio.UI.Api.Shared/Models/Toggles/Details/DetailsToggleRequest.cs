using MediatR;

namespace Esquio.UI.Api.Shared.Models.Toggles.Details
{
    public class DetailsToggleRequest : IRequest<DetailsToggleResponse>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string ToggleType { get; set; }
    }
}

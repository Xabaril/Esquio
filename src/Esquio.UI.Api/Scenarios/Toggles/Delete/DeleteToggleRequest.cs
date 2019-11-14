using MediatR;

namespace Esquio.UI.Api.Features.Toggles.Delete
{
    public class DeleteToggleRequest : IRequest
    {
        public string ToggleType { get; set; }

        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}

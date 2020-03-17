using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.Delete
{
    public class DeleteFeatureRequest : IRequest
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }
    }
}

using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.Details
{
    public class DetailsFeatureRequest : IRequest<DetailsFeatureResponse>
    {
        public DetailsFeatureRequest(string productName, string featureName)
        {
            ProductName = productName;
            FeatureName = featureName;
        }

        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}

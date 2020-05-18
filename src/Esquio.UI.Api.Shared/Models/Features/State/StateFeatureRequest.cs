using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.State
{
    public class StateFeatureRequest
        :IRequest<StateFeatureResponse>
    {
        public StateFeatureRequest(string productName, string featureName)
        {
            ProductName = productName;
            FeatureName = featureName;
        }
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}

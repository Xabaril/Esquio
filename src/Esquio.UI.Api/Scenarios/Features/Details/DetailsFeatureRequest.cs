using MediatR;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFeatureRequest : IRequest<DetailsFeatureResponse>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}

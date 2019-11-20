using MediatR;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteFeatureRequest : IRequest
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }
    }
}

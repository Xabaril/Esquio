using MediatR;

namespace Esquio.UI.Api.Scenarios.Flags.Archive
{
    public class ArchiveFeatureRequest
     : IRequest
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }
    }
}

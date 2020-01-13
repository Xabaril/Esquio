using MediatR;

namespace Esquio.UI.Api.Features.Flags.Archive
{
    public class ArchiveFeatureRequest
     : IRequest
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }
    }
}

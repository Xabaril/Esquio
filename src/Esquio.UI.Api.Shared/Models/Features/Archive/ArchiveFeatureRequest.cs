using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.Archive
{
    public class ArchiveFeatureRequest
     : IRequest
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }
    }
}

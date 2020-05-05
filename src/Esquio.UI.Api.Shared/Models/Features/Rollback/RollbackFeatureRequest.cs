using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.Rollback
{
    public class RollbackFeatureRequest
     : IRequest
    {
        public string FeatureName { get; set; }

        public string RingName { get; set; }

        public string ProductName { get; set; }
    }
}

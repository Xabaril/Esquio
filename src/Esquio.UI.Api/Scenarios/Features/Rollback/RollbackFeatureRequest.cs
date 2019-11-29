using MediatR;

namespace Esquio.UI.Api.Features.Flags.Rollback
{
    public class RollbackFeatureRequest
     : IRequest
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }
    }
}

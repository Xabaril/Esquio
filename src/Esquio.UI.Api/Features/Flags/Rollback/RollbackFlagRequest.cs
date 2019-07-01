using MediatR;

namespace Esquio.UI.Api.Features.Flags.Rollback
{
    public class RollbackFlagRequest
     : IRequest
    {
        public int FeatureId { get; set; }
    }
}

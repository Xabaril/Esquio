using MediatR;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteFlagRequest : IRequest
    {
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
    }
}

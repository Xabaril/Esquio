using MediatR;

namespace Esquio.UI.Api.Scenarios.Products.DeleteRing
{
    public class DeleteRingRequest : IRequest<Unit>
    {
        internal string ProductName { get; set; }

        internal string RingName { get; set; }
    }
}

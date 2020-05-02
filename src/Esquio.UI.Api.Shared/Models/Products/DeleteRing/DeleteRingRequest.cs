using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.DeleteRing
{
    public class DeleteRingRequest : IRequest<Unit>
    {
        internal string ProductName { get; set; }

        internal string RingName { get; set; }
    }
}

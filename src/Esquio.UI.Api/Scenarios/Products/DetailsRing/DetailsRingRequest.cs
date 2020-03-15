using MediatR;

namespace Esquio.UI.Api.Scenarios.Products.DetailsRing
{
    public class DetailsRingRequest : IRequest<DetailsRingResponse>
    {
        public string ProductName { get; set; }
    }
}

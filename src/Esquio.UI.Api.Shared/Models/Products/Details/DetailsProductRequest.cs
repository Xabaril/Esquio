using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.Details
{
    public class DetailsProductRequest : IRequest<DetailsProductResponse>
    {
        public string ProductName { get; set; }
    }
}

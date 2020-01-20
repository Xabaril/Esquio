using MediatR;

namespace Esquio.UI.Api.Scenarios.Products.Delete
{
    public class DeleteProductRequest : IRequest
    {
        public string ProductName { get; set; }
    }
}

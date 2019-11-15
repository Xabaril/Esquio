using MediatR;

namespace Esquio.UI.Api.Features.Products.Add
{
    public class AddProductRequest : IRequest<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

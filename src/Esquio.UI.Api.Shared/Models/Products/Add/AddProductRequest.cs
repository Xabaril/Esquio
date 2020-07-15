using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.Add
{
    public class AddProductRequest : IRequest<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultDeploymentName { get; set; }
    }
}

using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.AddDeployment
{
    public class AddDeploymentRequest : IRequest<Unit>
    {
        internal string ProductName { get; set; }
        public string Name { get; set; }
    }
}

using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.DeleteDeployment
{
    public class DeleteDeploymentRequest : IRequest<Unit>
    {
        internal string ProductName { get; set; }

        internal string DeploymentName { get; set; }
    }
}

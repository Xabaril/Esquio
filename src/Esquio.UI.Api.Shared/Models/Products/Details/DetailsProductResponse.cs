using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Products.Details
{
    public class DetailsProductResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<DeploymentResponseDetail> Deployments { get; set; }
    }
    public class DeploymentResponseDetail
    {
        public string Name { get; set; }

        public bool Default { get; set; }
    }
}

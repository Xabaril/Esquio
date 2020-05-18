using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class DeploymentEntityBuilder
    {
        private int _productId = 1;
        private string _name = "deployment-name";
        private bool _default = false;

        public DeploymentEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public DeploymentEntityBuilder WithProduct(ProductEntity product)
        {
            _productId = product.Id;
            return this;
        }

        public DeploymentEntityBuilder WithDefault(bool @default)
        {
            _default = @default;
            return this;
        }

        public DeploymentEntity Build()
        {
            return new DeploymentEntity(productEntityId: _productId, name: _name, byDefault: _default);
        }
    }
}

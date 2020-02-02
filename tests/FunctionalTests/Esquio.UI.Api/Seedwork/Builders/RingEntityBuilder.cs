using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class RingEntityBuilder
    {
        private int _productId = 1;
        private string _name = "ring-name";
        private bool _default = false;

        public RingEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public RingEntityBuilder WithProduct(ProductEntity product)
        {
            _productId = product.Id;
            return this;
        }

        public RingEntityBuilder WithDefault(bool @default)
        {
            _default = @default;
            return this;
        }

        public RingEntity Build()
        {
            return new RingEntity(productEntityId: _productId, name: _name, byDefault: _default);
        }
    }
}

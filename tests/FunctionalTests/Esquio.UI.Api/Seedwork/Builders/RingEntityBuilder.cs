using Esquio.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class RingEntityBuilder
    {
        private int _productId = 1;
        private string _name = "ring-name";

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

        public RingEntity Build()
        {
            return new RingEntity(_productId, _name);
        }
    }
}

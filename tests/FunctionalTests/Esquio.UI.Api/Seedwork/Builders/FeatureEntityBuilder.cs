using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class FeatureEntityBuilder
    {
        private int _productId = 1;
        private string _name = "feature-name";
        private bool _archived = false;

   
        public FeatureEntityBuilder WithArchived(bool archived = false)
        {
            _archived = archived;
            return this;
        }

        public FeatureEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public FeatureEntityBuilder WithProduct(ProductEntity product)
        {
            _productId = product.Id;
            return this;
        }

        public FeatureEntity Build()
        {
            return new FeatureEntity(_productId, _name, _archived);
        }
    }
}

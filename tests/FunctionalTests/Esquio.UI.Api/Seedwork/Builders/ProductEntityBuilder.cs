using Esquio.EntityFrameworkCore.Store.Entities;
using System.Collections.Generic;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ProductEntityBuilder
    {
        private string _name;
        private List<FeatureEntity> _features = new List<FeatureEntity>();

        public ProductEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public ProductEntityBuilder WithFeature(FeatureEntity featureEntity)
        {
            _features.Add(featureEntity);

            return this;
        }
        public ProductEntity Build()
        {
            var entity = new ProductEntity(_name);
            
            foreach(var item in _features)
            {
                entity.Features.Add(item);
            }

            return entity;
        }
    }
}

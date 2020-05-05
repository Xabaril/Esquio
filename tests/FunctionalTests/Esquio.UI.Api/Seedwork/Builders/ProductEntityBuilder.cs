using Esquio.UI.Api.Infrastructure.Data.Entities;
using System.Collections.Generic;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ProductEntityBuilder
    {
        private string _name;
        private List<FeatureEntity> _features = new List<FeatureEntity>();
        private List<DeploymentEntity> _rings = new List<DeploymentEntity>();

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

        public ProductEntityBuilder WithRing(DeploymentEntity ringEntity)
        {
            _rings.Add(ringEntity);
            return this;
        }

        public ProductEntity Build()
        {
            var entity = new ProductEntity(_name);
            
            foreach(var item in _features)
            {
                entity.Features.Add(item);
            }

            foreach (var item in _rings)
            {
                entity.Deployments.Add(item);
            }

            return entity;
        }
    }
}

using Esquio.Abstractions;
using Esquio.Model;
using System;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    class DelegatedValueFeatureStore
        : IFeatureStore
    {
        private readonly Func<string, string, Feature> _getDelegatedFeatureFunc;

        public bool IsReadOnly => true;

        public DelegatedValueFeatureStore(Func<string, string, Feature> getDelegatedFeatureFunc)
        {
            _getDelegatedFeatureFunc = getDelegatedFeatureFunc;
        }
        public Task AddFeatureAsync(string productName, Feature feature)
        {
            throw new NotImplementedException();
        }

        public Task AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Feature> FindFeatureAsync(string featureName, string productName)
        {
            return Task.FromResult(_getDelegatedFeatureFunc(featureName, productName));
        }

        public Task<Product> FindProductAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFeatureAsync(string product, Feature feature)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFeatureAsync(string product, Feature feature)
        {
            throw new NotImplementedException();
        }
    }
}

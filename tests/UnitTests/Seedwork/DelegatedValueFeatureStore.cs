using Esquio.Abstractions;
using Esquio.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    class DelegatedValueFeatureStore
        : IRuntimeFeatureStore
    {
        private readonly Func<string, string, string, Feature> _getDelegatedFeatureFunc;

        public bool IsReadOnly => true;

        public DelegatedValueFeatureStore(Func<string, string, string, Feature> getDelegatedFeatureFunc)
        {
            _getDelegatedFeatureFunc = getDelegatedFeatureFunc;
        }


        public Task<Feature> FindFeatureAsync(string featureName, string productName, string deploymentName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_getDelegatedFeatureFunc(featureName, productName,deploymentName));
        }
    }
}

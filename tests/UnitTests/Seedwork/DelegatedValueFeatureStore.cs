using Esquio.Abstractions;
using Esquio.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    class DelegatedValueFeatureStore
        : IRuntimeFeatureStore
    {
        private readonly Func<string, string, Feature> _getDelegatedFeatureFunc;

        public bool IsReadOnly => true;

        public DelegatedValueFeatureStore(Func<string, string, Feature> getDelegatedFeatureFunc)
        {
            _getDelegatedFeatureFunc = getDelegatedFeatureFunc;
        }


        public Task<Feature> FindFeatureAsync(string featureName, string productName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_getDelegatedFeatureFunc(featureName, productName));
        }
    }
}

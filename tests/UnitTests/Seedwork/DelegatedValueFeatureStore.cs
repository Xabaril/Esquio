using Esquio.Abstractions;
using Esquio.Model;
using System;
using System.Collections.Generic;
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
        

        public Task<Feature> FindFeatureAsync(string featureName, string productName)
        {
            return Task.FromResult(_getDelegatedFeatureFunc(featureName, productName));
        }

        public Task<IEnumerable<Feature>> FindUserPreviewFeatures(string productName = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EnableUserPreviewFeature(string featureName, string userName, string productName = null)
        {
            throw new NotImplementedException();
        }
    }
}

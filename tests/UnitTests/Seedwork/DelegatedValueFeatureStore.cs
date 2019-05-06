using Esquio.Abstractions;
using Esquio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    class DelegatedValueFeatureStore
        : IFeatureStore
    {
        private readonly Func<string, object> _getDelegatedValueFunc;

        public bool IsReadOnly => true;

        public DelegatedValueFeatureStore(Func<string, object> getDelegatedValueFunc)
        {
            _getDelegatedValueFunc = getDelegatedValueFunc;
        }
        public Task<object> GetToggleParameterValueAsync<TToggle>(string applicationName, string featureName, string parameterName) where TToggle : IToggle
        {
            return Task.FromResult(_getDelegatedValueFunc(parameterName));
        }
        public Task<bool> AddFeatureAsync(string applicationName, string featureName, bool enabled = false)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AddToggleAsync<TToggle>(string applicationName, string featureName, IDictionary<string, object> parameterValues) where TToggle : IToggle
        {
            throw new NotImplementedException();
        }

        public Task<Feature> FindFeatureAsync(string applicationName, string featureName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> FindTogglesTypesAsync(string applicationName, string featureName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddFeatureAsync(string applicationName, Feature feature)
        {
            throw new NotImplementedException();
        }
    }
}

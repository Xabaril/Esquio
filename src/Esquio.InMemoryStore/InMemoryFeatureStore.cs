using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esquio.Abstractions;

namespace Esquio.InMemoryStore
{
    public class InMemoryFeatureStore
        : IFeatureStore
    {
        public Task<bool> AddFeatureAsync(string application, string feature, bool enabled = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddToggleAsync<TToggle>(string application, string feature, IDictionary<string, object> parameterValues) where TToggle : IToggle
        {
            throw new System.NotImplementedException();
        }

        public Task<Feature> FindFeatureAsync(string applicationName, string featureName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Type>> FindTogglesTypesAsync(string applicationName, string featureName)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetParameterValueAsync<TToggle>(string application, string feature, string parameterName) where TToggle : IToggle
        {
            throw new System.NotImplementedException();
        }
    }
}

using Esquio.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.Configuration.Store
{
    public class ConfigurationFeatureStore : IFeatureStoreReadOnly
    {
        private readonly IConfigurationSection _configuration;

        public ConfigurationFeatureStore(IConfigurationSection configuration)
        {
            _configuration = configuration;
        }

        public Task<Feature> FindFeatureAsync(string applicationName, string featureName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Type>> FindTogglesTypesAsync(string applicationName, string featureName)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetParameterValueAsync<TToggle>(string applicationName, string featureName, string parameterName) where TToggle : IToggle
        {
            throw new NotImplementedException();
        }
    }
}

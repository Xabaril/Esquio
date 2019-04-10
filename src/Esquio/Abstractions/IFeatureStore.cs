using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IFeatureStore
    {
        Task<bool> AddFeatureAsync(string applicationName, string featureName, bool enabled = false);
        Task<bool> AddToggleAsync<TToggle>(string applicationName, string featureName, IDictionary<string, object> parameterValues)
             where TToggle : IToggle;
        Task<object> GetParameterValueAsync<TToggle>(string applicationName, string featureName, string parameterName)
            where TToggle : IToggle;

        Task<Feature> FindFeatureAsync(string applicationName, string featureName);

        Task<IEnumerable<Type>> FindTogglesTypesAsync(string applicationName, string featureName);
    }
}

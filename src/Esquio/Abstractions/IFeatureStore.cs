using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IFeatureStore
    {
        bool IsReadOnly { get; }
        Task<bool> AddFeatureAsync(string applicationName, string featureName, bool enabled = false);
        Task<Feature> FindFeatureAsync(string applicationName, string featureName);
        Task<bool> AddToggleAsync<TToggle>(string applicationName, string featureName, IDictionary<string, object> parameterValues)
             where TToggle : IToggle;
        Task<object> GetToggleParameterValueAsync<TToggle>(string applicationName, string featureName, string parameterName)
            where TToggle : IToggle;
        Task<IEnumerable<string>> FindTogglesTypesAsync(string applicationName, string featureName);
    }
}

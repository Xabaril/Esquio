using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IFeatureStore
    {
        bool IsReadOnly { get; }
        Task<bool> AddFeatureAsync(string featureName, string applicationName, bool enabled = false);
        Task<Feature> FindFeatureAsync(string featureName, string applicationName);
        Task<bool> AddToggleAsync<TToggle>(string featureName, string applicationName, IDictionary<string, object> parameterValues)
             where TToggle : IToggle;
        Task<object> GetToggleParameterValueAsync<TToggle>(string featureName, string applicationName, string parameterName)
            where TToggle : IToggle;
        Task<IEnumerable<string>> FindTogglesTypesAsync(string featureName, string applicationName);
    }
}

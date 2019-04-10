using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IFeatureStore
    {
        Task<bool> AddFeatureAsync(string application, string feature, bool enabled = false);
        Task<bool> AddToggleAsync<TToggle>(string application, string feature, IDictionary<string, object> parameterValues)
             where TToggle : IToggle;
        Task<object> GetParameterValueAsync<TToggle>(string application, string feature, string parameterName)
            where TToggle : IToggle;


    }
}

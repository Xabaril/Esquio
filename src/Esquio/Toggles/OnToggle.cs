using Esquio.Abstractions;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    public class OnToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string applicationName, string featureName)
        {
            return Task.FromResult<bool>(true);
        }
    }
}

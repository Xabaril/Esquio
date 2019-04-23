using Esquio.Abstractions;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    public class OnToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            return Task.FromResult<bool>(true);
        }
    }
}

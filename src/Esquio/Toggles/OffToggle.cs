using Esquio.Abstractions;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    public class OffToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            return Task.FromResult<bool>(false);
        }
    }
}

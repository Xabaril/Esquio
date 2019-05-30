using Esquio.Abstractions;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that never is active.")]
    public class OffToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            return Task.FromResult<bool>(false);
        }
    }
}

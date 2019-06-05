using Esquio.Abstractions;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that always is active.")]
    public class OnToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string featureName, string productName = null)
        {
            return Task.FromResult<bool>(true);
        }
    }
}

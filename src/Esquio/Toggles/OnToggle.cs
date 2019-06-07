using Esquio.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that always is active.")]
    public class OnToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<bool>(true);
        }
    }
}

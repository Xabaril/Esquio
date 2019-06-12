using Esquio.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that never is active.")]
    public class OffToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<bool>(false);
        }
    }
}

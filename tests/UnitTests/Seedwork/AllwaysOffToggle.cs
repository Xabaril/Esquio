using Esquio.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    public class AllwaysOffToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}

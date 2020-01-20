using Esquio.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    public class AllwaysOnToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}

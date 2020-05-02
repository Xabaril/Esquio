using Esquio.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    public class AllwaysOffToggle
        : IToggle
    {
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            return new ValueTask<bool>(false);
        }
    }
}

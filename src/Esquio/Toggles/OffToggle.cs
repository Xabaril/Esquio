using Esquio.Abstractions;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    public class OffToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(IFeatureContext context)
        {
            return Task.FromResult<bool>(false);
        }
    }
}

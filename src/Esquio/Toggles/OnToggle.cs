using Esquio.Abstractions;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    public class OnToggle
        : IToggle
    {
        public Task<bool> IsActiveAsync(IFeatureContext context)
        {
            return Task.FromResult<bool>(true);
        }
    }
}

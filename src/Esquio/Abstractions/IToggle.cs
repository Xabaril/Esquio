using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IToggle
    {
        Task<bool> IsActiveAsync(IFeatureContext context);
    }
}

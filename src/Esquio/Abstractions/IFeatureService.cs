using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IFeatureService
    {
        Task<bool> IsEnabledAsync(string application, string feature);
    }
}

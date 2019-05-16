using Esquio.Model;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IRuntimeFeatureStore
    {
        Task<Feature> FindFeatureAsync(string featureName, string productName = null);
    }
}

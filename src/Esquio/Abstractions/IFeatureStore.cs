using Esquio.Model;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IFeatureStore
    {
        bool IsReadOnly { get; }
        Task<bool> AddFeatureAsync(string product, Feature feature);
        Task<bool> AddFeatureAsync(string featureName, string product, bool enabled = false);
        Task<Feature> FindFeatureAsync(string featureName, string product);
    }
}

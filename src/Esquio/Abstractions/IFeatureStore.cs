using Esquio.Model;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public interface IFeatureStore
    {
        bool IsReadOnly { get; }
        Task<Product> FindProductAsync(string name);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task AddFeatureAsync(string product, Feature feature);
        Task<Feature> FindFeatureAsync(string featureName, string productName);
    }
}

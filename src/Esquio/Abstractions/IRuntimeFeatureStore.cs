using Esquio.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// The base contract thtat represent the runtime feature store used to find feature data.
    /// </summary>
    public interface IRuntimeFeatureStore
    {
        /// <summary>
        /// Get the specified feature from the store if exist,else return null.
        /// </summary>
        /// <param name="featureName">The feature name used to find in the store.</param>
        /// <param name="productName">The produce when the feature is configured.</param>
        /// <param name="cancellationToken"> A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>The feature data from the store or null if feature does not exist.</returns>
        /// <returns>A <see cref="Task{Feature}"/> that complete when service finished, yielding the Feature with name  <paramref name="featureName"/> or null if feature does not exist.</returns>
        Task<Feature> FindFeatureAsync(string featureName, string productName = null, CancellationToken cancellationToken = default);
    }
}

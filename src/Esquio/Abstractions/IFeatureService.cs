using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// The base contract for the feature service. Use this service to check if any feature on some product is enabled or not.
    /// </summary>
    public interface IFeatureService
    {
        /// <summary>
        /// Check if the specified feature <paramref name="featureName"/> on the application <paramref name="productName"/> is 
        /// enabled or not.
        /// </summary>
        /// <param name="featureName">The specified feature name.</param>
        /// <param name="cancellationToken"> A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{bool}"/> that complete when service finished, yielding True if feature <paramref name="featureName"/> is enabled on application <paramref name="productName"/>.</returns>
        Task<bool> IsEnabledAsync(string featureName, CancellationToken cancellationToken = default);
    }
}

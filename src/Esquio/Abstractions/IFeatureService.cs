using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// The base contract for the Feature Service.
    /// </summary>
    public interface IFeatureService
    {
        /// <summary>
        /// Check if the specified feature <paramref name="featureName"/> on the application <paramref name="productName"/> is 
        /// active or not.
        /// </summary>
        /// <param name="featureName">The specified feature name.</param>
        /// <param name="productName">The specified product name or null for default application.</param>
        /// <param name="cancellationToken"> A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{bool}"/> that complete when service finished, yielding True if feature <paramref name="featureName"/> is active on application <paramref name="productName"/>.</returns>
        Task<bool> IsEnabledAsync(string featureName, string productName = null, CancellationToken cancellationToken = default);
    }
}

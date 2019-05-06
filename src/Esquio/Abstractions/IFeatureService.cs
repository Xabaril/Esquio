using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// The base contract for the Feature Service.
    /// </summary>
    public interface IFeatureService
    {
        /// <summary>
        /// Check if the specified feature <paramref name="featureName"/> on the application <paramref name="applicationName"/> is 
        /// active or not.
        /// </summary>
        /// <param name="featureName">The specified feature name.</param>
        /// <param name="applicationName">The specified application name or null for default application.</param>
        /// <returns>A <see cref="Task{bool}"/> that complete when service finished, yielding True if feature <paramref name="featureName"/> is active on application <paramref name="applicationName"/>.</returns>
        Task<bool> IsEnabledAsync(string featureName, string applicationName = null);
    }
}

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
        /// <param name="applicationName">The specified application name.</param>
        /// <param name="featureName">The specified feature name.</param>
        /// <returns>True if feature <paramref name="featureName"/> is active on application <paramref name="applicationName"/>.</returns>
        Task<bool> IsEnabledAsync(string applicationName, string featureName);
    }
}

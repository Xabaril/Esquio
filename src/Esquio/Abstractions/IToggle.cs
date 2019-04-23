using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent the base contract for all Toggles.
    /// </summary>
    public interface IToggle
    {
        /// <summary>
        /// Check if the toggle is active for an application and feature.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <param name="applicationName">The application name.Optional</param>
        /// <returns></returns>
        Task<bool> IsActiveAsync(string featureName, string applicationName = null);
    }
}

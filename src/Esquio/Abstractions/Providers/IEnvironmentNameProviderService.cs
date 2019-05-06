using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{

    /// <summary>
    /// Base contract for get current environment name.
    /// </summary>
    public interface IEnvironmentNameProviderService
    {
        /// <summary>
        /// Get current environment name.
        /// </summary>
        /// <returns>A <see cref="Task{string}"/> that complete when the provider has finished, yielding the name of the current environment.</returns>
        Task<string> GetEnvironmentNameAsync();
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// Base contract for discover the Environment of the current application.
    /// </summary>
    public interface IEnvironmentNameProviderService
    {
        /// <summary>
        /// Get current environment name.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete. </param>
        /// <returns>A <see cref="Task{string}"/> that complete when the provider has finished, yielding the name of the current environment.</returns>
        Task<string> GetEnvironmentNameAsync(CancellationToken cancellationToken = default);
    }

    internal sealed class NoEnvironmentNameProviderService
        : IEnvironmentNameProviderService
    {
        public Task<string> GetEnvironmentNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<string>(null);
        }
    }
}

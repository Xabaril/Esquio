using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// Base contract used to get the current User name when is necessary on <see cref="IToggle"/> executions.
    /// In ASP.NET Core this contract is implemented using current HttpContext.User.Identity. For non HttpContext
    /// you can implement how the Role is discovered.
    public interface IUserNameProviderService
    {
        /// <summary>
        /// Get the current user name.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete. </param>
        /// <returns>A <see cref="Task{string}"/> that complete when the provider service has finished, yielding the current username.</returns>
        Task<string> GetCurrentUserNameAsync(CancellationToken cancellationToken = default);
    }
    internal sealed class NoUserNameProviderService
        : IUserNameProviderService
    {
        public Task<string> GetCurrentUserNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<string>(null);
        }
    }
}

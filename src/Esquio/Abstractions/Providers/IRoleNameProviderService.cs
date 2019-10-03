using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// Base contract used to get the current Role name when is necessary on <see cref="IToggle"/> executions.
    /// In ASP.NET Core this contract is implemented using current HttpContext.User.Identity. For non HttpContext
    /// you can implement how the Role is discovered.
    /// </summary>
    public interface IRoleNameProviderService
    {
        /// <summary>
        /// Get the current role.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{string}"/> that complete when the provider service has finished, yielding the current role name.</returns>
        Task<string> GetCurrentRoleNameAsync(CancellationToken cancellationToken = default);
    }

    internal sealed class NoRoleNameProviderService
        : IRoleNameProviderService
    {
        public Task<string> GetCurrentRoleNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<string>(null);
        }
    }
}

using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// base contract for role provider.
    /// </summary>
    public interface IRoleNameProviderService
    {
        /// <summary>
        /// Get the current role.
        /// </summary>
        /// <returns>A <see cref="Task{string}"/> that complete when the provider service has finished, yielding the current role name.</returns>
        Task<string> GetCurrentRoleNameAsync();
    }
}

using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// Base contract for username provider.
    /// </summary>
    public interface IUserNameProviderService
    {
        /// <summary>
        /// Get the current user name.
        /// </summary>
        /// <returns>A <see cref="Task{string}"/> that complete when the provider service has finished, yielding the current username.</returns>
        Task<string> GetCurrentUserNameAsync();
    }
}

using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// Base contract for the UserNameProviderService used on different Toggles.
    /// </summary>
    public interface IUserNameProviderService
    {
        /// <summary>
        /// Get the current UserName.
        /// </summary>
        /// <returns>The current UserName.</returns>
        Task<string> GetCurrentUserNameAsync();
    }
}

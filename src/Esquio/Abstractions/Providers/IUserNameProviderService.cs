using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    public interface IUserNameProviderService
    {
        Task<string> GetCurrentUserNameAsync();
    }
}

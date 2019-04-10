using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    public interface IRoleNameProviderService
    {
        Task<string> GetCurrentRoleNameAsync();
    }
}

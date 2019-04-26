using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    public interface IEnvironmentNameProviderService
    {
        Task<string> GetEnvironmentNameAsync();
    }
}

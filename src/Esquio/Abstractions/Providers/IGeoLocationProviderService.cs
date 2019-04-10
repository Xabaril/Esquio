using System.Net;
using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    public interface IGeoLocationProviderService
    {
        Task<string> GetIsoCountryName(IPAddress ipAddress);
        Task<string> GetIsoCountryName(string ipAddress);
    }
}

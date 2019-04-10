using System.Net;
using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    public class NoGeoLocationProviderService
        : IGeoLocationProviderService
    {
        public Task<string> GetIsoCountryName(IPAddress ipAddress)
        {
            return Task.FromResult<string>(null);
        }
        public Task<string> GetIsoCountryName(string ipAddress)
        {
            return Task.FromResult<string>(null);
        }
    }
}

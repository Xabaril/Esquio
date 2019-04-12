using System.Net;
using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// Base contract for Geo-Location provider service.
    /// </summary>
    public interface IGeoLocationProviderService
    {
        /// <summary>
        /// Get the ISO country name from the specified ip <paramref name="ipAddress"/>
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>The ISO country name location of <paramref name="ipAddress"/></returns>
        Task<string> GetIsoCountryName(IPAddress ipAddress);

        /// <summary>
        /// Get the ISO country name from the specified ip <paramref name="ipAddress"/>
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>The ISO country name location of <paramref name="ipAddress"/></returns>
        Task<string> GetIsoCountryName(string ipAddress);
    }
}

using System.Net;
using System.Threading.Tasks;

namespace Esquio.Abstractions.Providers
{
    /// <summary>
    /// Default <see cref="IGeoLocationProviderService"/> implementation.
    /// </summary>
    public class NoGeoLocationProviderService
        : IGeoLocationProviderService
    {
        /// <summary>
        /// Get null ISO country name from <paramref name="ipAddress"/> address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>A <see cref="Task{string}"/> that complete when service finished, yielding null.</returns>
        public Task<string> GetIsoCountryName(IPAddress ipAddress)
        {
            return Task.FromResult<string>(null);
        }

        /// <summary>
        /// Get null ISO country name from <paramref name="ipAddress"/> adddress.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>A <see cref="Task{string}"/> that complete when service finished, yielding null.</returns>
        public Task<string> GetIsoCountryName(string ipAddress)
        {
            return Task.FromResult<string>(null);
        }
    }
}

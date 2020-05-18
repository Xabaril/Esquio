using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on request ip.
    /// </summary>
    [DesignType(Description = "The request country is in the list (Ip geolocation through https://ip2c.org service).", FriendlyName = "Country")]
    [DesignTypeParameter(ParameterName = Countries, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The request country values (two letters, ISO 3166) to activate this toggle separated by ';' character.")]
    public class Ip2CountryToggle
       : IToggle
    {
        private const string Countries = nameof(Countries);

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="httpClientFactory">The http client factory.</param>
        public Ip2CountryToggle(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _contextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        ///<inheritdoc/>
        public async ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var allowedCountries = context.Data[Countries]?.ToString();
            var currentCountry = await GetCurrentCountry();

            if (allowedCountries != null && !String.IsNullOrEmpty(currentCountry))
            {
                var tokenizer = new StringTokenizer(allowedCountries, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                foreach (var segment in tokenizer)
                {
                    if (segment.Value.IndexOf(currentCountry, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<string> GetCurrentCountry()
        {
            var remoteIpAddress = GetRemoteIpAddress();

            var ip = (long)(uint)IPAddress.NetworkToHostOrder(
                BitConverter.ToInt32(remoteIpAddress.GetAddressBytes(), 0));

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"https://ip2c.org/{ip}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                //see https://about.ip2c.org/#outputs to get more information about ip2c output's result

                var ip2c = content.Split(EsquioConstants.DEFAULT_SPLIT_SEPARATOR);
                if (ip2c.Any()
                    &&
                    ip2c.Length == 4
                    &&
                    ip2c[0] == "1")
                {
                    //we use ISO 3166 output ( two letters )
                    return ip2c[1];
                }
            }

            return null;
        }

        private IPAddress GetRemoteIpAddress()
        {
            var request = _contextAccessor
                .HttpContext
                .Request;

            var header = request
                .Headers
                .FirstOrDefault(h => h.Key == "CF-Connecting-IP").Value;

            if (header == default(StringValues))
            {
                header = request
                    .Headers
                    .FirstOrDefault(h => h.Key == "X-Forwarded-For").Value;
            }

            if (header != default(StringValues))
            {
                var headerValue = header.First();
                var ip = string.Empty;

                if ( headerValue.IndexOf(',') > 0)
                {
                    ip =  headerValue.Split(',')[0];
                }
                else
                {
                    ip = headerValue;
                }

                return IPAddress.Parse(ip);
            }
            else
            {
                return _contextAccessor.HttpContext
                    .Connection
                    .RemoteIpAddress;
            }
        }
    }
}

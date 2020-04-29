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
    [DesignType(Description = "The request country is in the list (Ip geolocation through https://ip2c.org service).", FriendlyName = "Country")]
    [DesignTypeParameter(ParameterName = Countries, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The request country values (two letters, ISO 3166) to activate this toggle separated by ';' character.")]
    public class Ip2CountryToggle
    {
        private const string Countries = nameof(Countries);

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public Ip2CountryToggle(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _contextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

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
            var remoteIpAddress = _contextAccessor.HttpContext
                 .Connection
                 .RemoteIpAddress;

            var remoteIp = (long)(uint)IPAddress.NetworkToHostOrder(
                BitConverter.ToInt32(remoteIpAddress.GetAddressBytes(), 0));

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"https://ip2c.org/{remoteIp}");

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
    }
}

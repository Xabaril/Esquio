using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignType(Description = "The server IP address toggle activates a feature for ip addresses defined in the IP list.", FriendlyName = "Server IP")]
    [DesignTypeParameter(ParameterName = IpAddresses, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "Collection of IP addresses delimited by ';' character.")]
    public class ServerIpAddressToggle : IToggle
    {
        public const string IpAddresses = nameof(IpAddresses);

        private readonly IHttpContextAccessor _contextAccessor;

        public ServerIpAddressToggle(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var ipAddress = _contextAccessor.HttpContext
                    .Connection
                    .LocalIpAddress;

            if (ipAddress != null)
            {
                string allowedIpAddresses = context.Data[IpAddresses]?.ToString();

                var bytes = ipAddress.GetAddressBytes();

                var tokenizer = new StringTokenizer(allowedIpAddresses, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                foreach (var token in tokenizer)
                {
                    if (token.HasValue
                        && IPAddress.TryParse(token, out IPAddress address)
                        && address.GetAddressBytes().SequenceEqual(bytes))
                    {
                        return new ValueTask<bool>(true);
                    }
                }
            }

            return new ValueTask<bool>(false);
        }
    }
}

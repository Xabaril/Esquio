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
    [DesignType(Description = "The client IP address is in the list.", FriendlyName = "Client IP Address")]
    [DesignTypeParameter(ParameterName = IpAddresses, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The IP addresses to activate this toggle separated by ';' character.")]
    public class ClientIpAddressToggle : IToggle
    {
        public const string IpAddresses = nameof(IpAddresses);
        private static readonly char[] separators = new char[] { ';' };
        private readonly IHttpContextAccessor _contextAccessor;

        public ClientIpAddressToggle(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var ipAddress = _contextAccessor.HttpContext
                    .Connection
                    .RemoteIpAddress;

            if (ipAddress != null )
            {
                var bytes = ipAddress.GetAddressBytes();

                string allowedIpAddresses = context.Data[IpAddresses]?.ToString();
                var tokenizer = new StringTokenizer(allowedIpAddresses, separators);

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

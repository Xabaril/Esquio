using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignType(Description = "The hostname of the client instance is in the list.", FriendlyName = "Host name")]
    [DesignTypeParameter(ParameterName = HostNames, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The request connection hostnames values to activate this toggle separated by ';' character.")]
    public class HostNameToggle : IToggle
    {
        public const string HostNames = nameof(HostNames);

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HostNameToggle(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            string allowedHostNames = context.Data[HostNames]?.ToString();

            if ( allowedHostNames != null )
            {
                var hostName = _httpContextAccessor.HttpContext
                .Request.Host.Host;

                var tokenizer = new StringTokenizer(allowedHostNames, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                foreach (var token in tokenizer)
                {
                    if (token.HasValue && token.Value.Equals(hostName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new ValueTask<bool>(true);
                    }
                }
            }

            return new ValueTask<bool>(false);
        }
    }
}

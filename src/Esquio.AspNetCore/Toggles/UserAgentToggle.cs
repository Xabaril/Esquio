using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignType(Description = "Toggle that is active depending on request user agent browser information.", FriendlyName = "On Browser")]
    [DesignTypeParameter(ParameterName = Browsers, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "Collection of browser names delimited by ';' character.")]
    public class UserAgentToggle
        : IToggle
    {
        private const string Browsers = nameof(Browsers);

        private readonly IHttpContextAccessor _contextAccessor;

        public UserAgentToggle(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var allowedBrowsers = context.Data[Browsers]?.ToString();
            var currentBrowser = GetCurrentUserAgent();

            if (allowedBrowsers != null && !String.IsNullOrEmpty(currentBrowser))
            {
                var tokenizer = new StringTokenizer(allowedBrowsers, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                foreach (var segment in tokenizer)
                {
                    if (segment.Value.IndexOf(currentBrowser, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        return new ValueTask<bool>(true);
                    }
                }
            }

            return new ValueTask<bool>(false);
        }

        private string GetCurrentUserAgent()
        {
            const string UserAgent = "user-agent";

            return _contextAccessor.HttpContext
                .Request
                .Headers[UserAgent]
                .FirstOrDefault() ?? string.Empty;
        }
    }
}

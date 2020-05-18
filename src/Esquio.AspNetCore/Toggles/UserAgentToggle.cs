using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on user agent.
    /// </summary>
    [DesignType(Description = "The request user agent browser is in the list.", FriendlyName = "Browser")]
    [DesignTypeParameter(ParameterName = Browsers, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The user agents to activate this toggle separated by ';' character.")]
    public class UserAgentToggle
        : IToggle
    {
        private const string Browsers = nameof(Browsers);

        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor dependency.</param>
        public UserAgentToggle(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        ///<inheritdoc/>
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var allowedBrowsers = context.Data[Browsers]?.ToString();
            var currentBrowser = GetCurrentUserAgent();

            if (allowedBrowsers != null && !String.IsNullOrEmpty(currentBrowser))
            {
                var tokenizer = new StringTokenizer(allowedBrowsers, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                foreach (var segment in tokenizer)
                {
                    if (currentBrowser.IndexOf(segment.Value, StringComparison.InvariantCultureIgnoreCase) >= 0)
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

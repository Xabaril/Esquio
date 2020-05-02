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
    /// A binary <see cref="IToggle"/> that is active depending on the header value specified on HeaderName property.
    /// </summary>
    [DesignType(Description = "The request header exists and its value its in the list.", FriendlyName = "Http Header value")]
    [DesignTypeParameter(ParameterName = HeaderName, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The header name.")]
    [DesignTypeParameter(ParameterName = HeaderValues, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The header values to activate this toggle separated by ';' character.")]
    public class HeaderValueToggle
        : IToggle
    {
        internal const string HeaderName = nameof(HeaderName);
        internal const string HeaderValues = nameof(HeaderValues);

        private static char[] SPLIT_SEPARATOR = new char[] { ';' };

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> service to be used.</param>
        public HeaderValueToggle(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        ///<inheritdoc/>
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            string headerName = context.Data[HeaderName]?.ToString();
            string allowedValues = context.Data[HeaderValues]?.ToString();

            if (headerName != null
                &&
                allowedValues != null)
            {
                var tokenizer = new StringTokenizer(allowedValues, SPLIT_SEPARATOR);

                var values = _httpContextAccessor.HttpContext
                    .Request
                    .Headers[headerName];

                foreach (var item in values)
                {
                    var active = tokenizer.Contains(item, StringSegmentComparer.OrdinalIgnoreCase);

                    return new ValueTask<bool>(active);
                }
            }

            return new ValueTask<bool>(false);
        }
    }
}

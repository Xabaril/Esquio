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
    /// A binary <see cref="IToggle"/> that is active depending on the current value for the specified claim on ClaimType property.
    /// </summary>
    [DesignType(Description = "The claim of the current user exists and its value is in the list.", FriendlyName = "Identity Claim Value")]
    [DesignTypeParameter(ParameterName = ClaimType, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The claim type used to check value.")]
    [DesignTypeParameter(ParameterName = ClaimValues, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The claim value to check, multiple items separated by ';'.")]
    public class ClaimValueToggle
        : IToggle
    {
        internal const string ClaimType = nameof(ClaimType);
        internal const string ClaimValues = nameof(ClaimValues);

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> service to be used.</param>
        public ClaimValueToggle(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        ///<inheritdoc/>
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            string claimType = context.Data[ClaimType]?.ToString();
            string allowedValues = context.Data[ClaimValues]?.ToString();

            if (claimType != null
                &&
                allowedValues != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                if (user != null && user.Identity.IsAuthenticated)
                {
                    var value = user.FindFirst(claimType)?
                        .Value;

                    if (value != null)
                    {
                        var tokenizer = new StringTokenizer(allowedValues, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                        var active = tokenizer.Contains(
                            value, StringSegmentComparer.OrdinalIgnoreCase);

                        return new ValueTask<bool>(active);
                    }
                }
            }

            return new ValueTask<bool>(false);
        }
    }
}

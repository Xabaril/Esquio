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
    /// A binary <see cref="IToggle"/> that is active depending on the current User name and if this is contained in configured Users property.
    /// </summary>
    [DesignType(Description = "Toggle that is active depending on current user name.", FriendlyName = "ASP.NET Core Identity Username")]
    [DesignTypeParameter(ParameterName = Users, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The collection of user(s) to activate this toggle separated by ';' character")]
    public class UserNameToggle
        : IToggle
    {
        internal const string Users = nameof(Users);

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Create a new instance of <see cref="UserNameToggle"/>.
        /// </summary>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> service to be used.</param>
        public UserNameToggle(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        ///<inheritdoc/>
        public Task<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var currentUserName = GetCurrentUserName();

            if (currentUserName != null)
            {
                string activeUserNames = context.Data[Users]?.ToString();

                if (activeUserNames != null)
                {
                    var tokenizer = new StringTokenizer(activeUserNames, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);
                    var isActive = tokenizer.Contains(currentUserName, StringSegmentComparer.OrdinalIgnoreCase);

                    return Task.FromResult(isActive);
                }
            }

            return Task.FromResult(false);
        }

        private string GetCurrentUserName()
        {
            //TODO: buscarlo por la claim seleccionada 

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var userName = httpContext
                    .User?
                    .Identity?
                    .Name;

                return userName;
            }

            throw new InvalidOperationException($"HttpContext is null and {nameof(UserNameToggle)} can't recover the current User name for this provider.");
        }
    }
}

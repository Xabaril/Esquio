using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{

    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on the current Role name and if this is contained in configured Roles property.
    /// </summary>
    [DesignType(Description = "Toggle that is active depending on current user role name.", FriendlyName = "ASP.NET Core Identity Role")]
    [DesignTypeParameter(ParameterName = Roles, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The collection of rol(es) to activate this toggle separated by ';' character")]
    public class RoleNameToggle
       : IToggle
    {
        internal const string Roles = nameof(Roles);

        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Create a new instance of <see cref="RoleNameToggle"/>.
        /// </summary>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> service to be used.</param>
        public RoleNameToggle(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc/>
        public  ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var currentRole = GetCurrentRole();

            if (currentRole != null)
            {
                string activeRoles = context.Data[Roles]?.ToString();

                if (activeRoles != null)
                {
                    var tokenizer = new StringTokenizer(activeRoles, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);
                    var isActive = tokenizer.Contains(currentRole, StringSegmentComparer.OrdinalIgnoreCase);

                    return new ValueTask<bool>(isActive);
                }
            }

            return new ValueTask<bool>(false);
        }

        private string GetCurrentRole()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var roleClaimType = ClaimsIdentity.DefaultRoleClaimType;
                var roleName = default(string);

                var roleClaim = httpContext.User?
                    .FindFirst(roleClaimType);

                if (roleClaim != null)
                {
                    roleName = roleClaim.Value;
                }

                return roleName;
            }
            throw new InvalidOperationException($"HttpContext is null and {nameof(RoleNameToggle)} can't recover the current Role name for this provider.");
        }
    }
}

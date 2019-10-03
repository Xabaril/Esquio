using Esquio.Abstractions.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    internal sealed class AspNetCoreRoleNameProviderService
        : IRoleNameProviderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreRoleNameProviderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Task<string> GetCurrentRoleNameAsync(CancellationToken cancellationToken = default)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var roleName = default(string);

                var roleClaim = httpContext.User?
                    .FindFirst(ClaimTypes.Role);

                if (roleClaim != null)
                {
                    roleName = roleClaim.Value;
                }

                return Task.FromResult<string>(roleName);
            }

            throw new InvalidOperationException($"HttpContext is null and {nameof(AspNetCoreRoleNameProviderService)} can't recover the current Role name for this provider.");
        }
    }
}

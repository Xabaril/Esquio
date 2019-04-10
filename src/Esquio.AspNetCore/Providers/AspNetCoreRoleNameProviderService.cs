using Esquio.Abstractions.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    public sealed class AspNetCoreRoleNameProviderService
        : IRoleNameProviderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AspNetCoreRoleNameProviderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public Task<string> GetCurrentRoleNameAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var userName = httpContext.User
                    .Identity.Name;

                return Task.FromResult<string>(userName);
            }

            throw new InvalidOperationException($"HttpContext is null and {nameof(AspNetCoreRoleNameProviderService)} can't recover the current Role name for this provider.");
        }
    }
}

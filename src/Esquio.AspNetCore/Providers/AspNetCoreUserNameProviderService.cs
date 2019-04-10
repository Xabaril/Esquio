using Esquio.Abstractions.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    public sealed class AspNetCoreUserNameProviderService
        : IUserNameProviderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AspNetCoreUserNameProviderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public Task<string> GetCurrentUserNameAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var userName = httpContext.User
                    .Identity.Name;

                return Task.FromResult<string>(userName);
            }

            throw new InvalidOperationException($"HttpContext is null and {nameof(AspNetCoreUserNameProviderService)} can't recover the current User name for this provider.");
        }
    }
}

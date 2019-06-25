using Esquio.UI.Infrastructure.Security.ApiKey;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationBuilderExtensions
    {

        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyOptions> setup = null)
        {
            return authenticationBuilder.AddScheme<ApiKeyOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationDefaults.ApiKeyScheme, setup);
        }
    }

    public static class ApiKeyAuthenticationDefaults
    {
        public const string ApiKeyScheme = "ApiKey";
    }
}

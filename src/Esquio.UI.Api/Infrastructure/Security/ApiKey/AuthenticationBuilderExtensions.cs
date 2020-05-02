using Esquio.UI.Api.Infrastructure.Security.ApiKey;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyOptions> setup = null)
        {
            authenticationBuilder.Services
                .AddScoped<IApiKeyStore, DefaultApiKeyStore>();

            return authenticationBuilder.AddScheme<ApiKeyOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationDefaults.ApiKeyScheme, setup);
        }
    }

    public static class ApiKeyAuthenticationDefaults
    {
        public const string ApiKeyScheme = "ApiKey";
    }
}

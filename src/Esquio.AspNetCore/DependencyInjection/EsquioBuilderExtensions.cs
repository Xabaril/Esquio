using Esquio.Abstractions.Providers;
using Esquio.AspNetCore.Endpoints;
using Esquio.AspNetCore.Providers;
using Esquio.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class EsquioBuilderExtensions
    {
        /// <summary>
        /// Register default ASP.NET Core services for Esquio Abstractions. 
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <returns>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        /// <remarks>
        /// The registered service are:
        ///  - AspNetCoreUserProviderService that enable get user information from current <see cref="System.Security.Claims.ClaimsPrincipal"/>.
        ///  - AspNetCoreRoleNameProviderService that enable get role information from current <see cref="System.Security.Claims.ClaimsPrincipal"/>.
        ///  - AspNetEnvironmentNameProviderService that enable get enviornment name from current <see cref="Microsoft.AspNetCore.Hosting.IWebHostEnvironment"/>.
        /// </remarks>
        public static IEsquioBuilder AddAspNetCoreDefaultServices(this IEsquioBuilder builder)
        {
            builder.Services.AddTransient<IUserNameProviderService, AspNetCoreUserNameProviderService>();
            builder.Services.AddTransient<IRoleNameProviderService, AspNetCoreRoleNameProviderService>();
            builder.Services.AddSingleton<IEnvironmentNameProviderService, AspNetEnvironmentNameProviderService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, FeatureMatcherPolicy>());

            return builder;
        }

        public static IEsquioBuilder AddEndpointFallback(this IEsquioBuilder builder, RequestDelegate requestDelegate)
        {
            builder.Services.TryAddSingleton<EndpointFallbackService>(sp =>
            {
                return new EndpointFallbackService(requestDelegate);
            });

            return builder;
        }
    }
}

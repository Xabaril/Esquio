using Esquio.Abstractions.Providers;
using Esquio.AspNetCore.Endpoint;
using Esquio.AspNetCore.Mvc;
using Esquio.AspNetCore.Providers;
using Esquio.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

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
            builder.Services.AddTransient<IEnvironmentNameProviderService, AspNetEnvironmentNameProviderService>();
            builder.Services
                .TryAddSingleton<IMvcFallbackService>(sp =>
                {
                    return new DelegatedMvcFallbackService(_ => new NotFoundResult());
                });
            builder.Services.AddHttpContextAccessor();
            //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, FeatureMatcherPolicy>());
            return builder;
        }

        /// <summary>
        /// Register the <see cref="IActionResult"/> returned by <see cref="FeatureFilter"/> and endpoints selectors when any feature is not active.
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <param name="fallback">Action that create the <see cref="IActionResult"/> depending on the current <see cref="ResourceExecutingContext"/></param>
        /// <returns>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        public static IEsquioBuilder AddMvcFallbackAction(this IEsquioBuilder builder, Func<ResourceExecutingContext, IActionResult> fallback)
        {
            builder.Services
                .AddSingleton<IMvcFallbackService>(sp =>
                {
                    return new DelegatedMvcFallbackService(fallback);
                });

            return builder;
        }
    }
}

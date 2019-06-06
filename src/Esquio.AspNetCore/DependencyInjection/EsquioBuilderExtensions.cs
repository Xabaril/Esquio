using Esquio.Abstractions.Providers;
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
    public static class EsquioBuilderExtensions
    {
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

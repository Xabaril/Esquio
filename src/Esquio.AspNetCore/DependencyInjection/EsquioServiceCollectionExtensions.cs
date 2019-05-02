using Esquio;
using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.AspNetCore.Mvc;
using Esquio.AspNetCore.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EsquioServiceCollectionExtensions
    {
        public static IEsquioBuilder AddEsquio(this IServiceCollection serviceCollection, Action<EsquioOptions> configurationExpression = null)
        {
            var config = new EsquioOptions();
            configurationExpression?.Invoke(config);
            config.AssembliesToRegister.Add(typeof(EsquioServiceCollectionExtensions).Assembly);

            var builder = new EsquioBuilder(serviceCollection);
            builder.Services.AddTogglesFromAssemblies(config.AssembliesToRegister);
            builder.Services.AddTransient<IUserNameProviderService, AspNetCoreUserNameProviderService>();
            builder.Services.AddTransient<IRoleNameProviderService, AspNetCoreRoleNameProviderService>();
            builder.Services.AddTransient<IGeoLocationProviderService, NoGeoLocationProviderService>();
            builder.Services.AddTransient<IEnvironmentNameProviderService, AspNetEnvironmentNameProviderService>();
            builder.Services.AddScoped<IFeatureService, DefaultFeatureService>();
            builder.Services.AddScoped<IToggleTypeActivator, DefaultToggleTypeActivator>();
            builder.Services.TryAddSingleton<IMvcFallbackService>(sp =>
            {
                return new DelegatedMvcFallbackService(_ => new NotFoundResult());
            });
            builder.Services.AddHttpContextAccessor();

            return builder;
        }
    }
}

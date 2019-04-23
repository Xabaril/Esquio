using Esquio;
using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.AspNetCore.Providers;
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
            builder.Services.AddScoped<IFeatureService, DefaultFeatureService>();
            builder.Services.AddScoped<IToggleTypeActivator, DefaultToggleTypeActivator>();
            builder.Services.AddHttpContextAccessor();

            return builder;
        }
    }
}

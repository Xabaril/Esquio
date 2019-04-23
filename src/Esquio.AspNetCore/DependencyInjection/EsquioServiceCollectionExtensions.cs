using Esquio;
using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.AspNetCore.Providers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EsquioServiceCollectionExtensions
    {
        public static IEsquioBuilder AddEsquio(this IServiceCollection serviceCollection)
        {
            var builder = new EsquioBuilder(serviceCollection);

            builder.Services.AddTransient<IUserNameProviderService, AspNetCoreUserNameProviderService>();
            builder.Services.AddTransient<IRoleNameProviderService, AspNetCoreRoleNameProviderService>();
            builder.Services.AddTransient<IGeoLocationProviderService, NoGeoLocationProviderService>();
            builder.Services.AddScoped<IFeatureService, DefaultFeatureService>();
            builder.Services.AddScoped<IToggleTypeActivator, DefaultToggleTypeActivator>();
            builder.Services.AddScoped<IFeatureContextFactory>(sp =>
            {
                return new AspNetCoreFeatureContextFactory(sp);
            });

            builder.Services.AddHttpContextAccessor();

            return builder;
        }
    }
}

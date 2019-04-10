using Esquio;
using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.AspNetCore.Extensions;
using Esquio.AspNetCore.Providers;
using Esquio.InMemoryStore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EsquioServiceCollectionExtensions
    {
        public static IEsquioBuilder AddEsquio(this IServiceCollection serviceCollection)
        {
            var builder = new EsquioBuilder(serviceCollection);
            
            builder.Services.AddTransient<IFeatureService, DefaultFeatureService>();
            builder.Services.AddTransient<IUserNameProviderService, AspNetCoreUserNameProviderService>();
            builder.Services.AddTransient<IRoleNameProviderService, AspNetCoreRoleNameProviderService>();
            builder.Services.AddTransient<IGeoLocationProviderService, NoGeoLocationProviderService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IFeatureStore, InMemoryFeatureStore>();

            return builder;
        }
    }
}

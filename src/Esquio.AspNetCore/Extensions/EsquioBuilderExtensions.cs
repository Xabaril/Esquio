using Esquio.Abstractions.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Esquio.AspNetCore.Extensions
{
    public static class EsquioBuilderExtensions
    {
        public static IEsquioBuilder ReplaceUserNameProviderService<TProviderService>(this IEsquioBuilder builder)
            where TProviderService : class, IUserNameProviderService
        {
            builder.Services
                .AddTransient<IUserNameProviderService, TProviderService>();
            return builder;
        }
        public static IEsquioBuilder ReplaceRoleNameProviderService<TProviderService>(this IEsquioBuilder builder)
           where TProviderService : class, IRoleNameProviderService
        {
            builder.Services
                .AddTransient<IRoleNameProviderService, TProviderService>();
            return builder;
        }
        public static IEsquioBuilder ReplaceGeoLocationProviderService<TProviderService>(this IEsquioBuilder builder)
            where TProviderService : class ,IGeoLocationProviderService
        {
            builder.Services
                .AddTransient<IGeoLocationProviderService, TProviderService>();
            return builder;
        }
    }
}

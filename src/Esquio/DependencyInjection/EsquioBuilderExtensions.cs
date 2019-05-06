using Esquio.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EsquioBuilderExtensions
    {
        public static IEsquioBuilder Replace<TProviderService, TImplementationService>(this IEsquioBuilder builder)
            where TProviderService : class
            where TImplementationService : class, TProviderService
        {
            builder.Services
                .AddTransient<TProviderService, TImplementationService>();
            return builder;
        }
    }
}

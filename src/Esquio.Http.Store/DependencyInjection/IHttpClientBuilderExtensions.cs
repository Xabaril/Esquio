using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IHttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddHttpMessageHandler<TMessageHandler>(this IHttpClientBuilder builder, bool when)
            where TMessageHandler : DelegatingHandler
        {
            return when ? builder.AddHttpMessageHandler<TMessageHandler>() : builder;
        }
    }
}

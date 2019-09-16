using Esquio.Abstractions;
using Esquio.AspNetCore.ApplicationInsightProcessor;
using Esquio.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class EsquioBuilderExtensions
    {
        public static IEsquioBuilder AddApplicationInsightProcessor(this IEsquioBuilder builder)
        {
            //TODO: add all mandatory dependencies here! (processor and observer)

            builder.Services.AddScoped<IFeatureEvaluationObserver, HttpContextItemObserver>();

            return builder;
        }
    }
}

using Esquio.Abstractions;
using Esquio.AspNetCore.ApplicationInsightProcessor;
using Esquio.AspNetCore.ApplicationInsightProcessor.Processor;
using Esquio.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class EsquioBuilderExtensions
    {
        public static IEsquioBuilder AddApplicationInsightProcessor(this IEsquioBuilder builder)
        {
            builder.Services.AddApplicationInsightsTelemetryProcessor<EsquioProcessor>();
            builder.Services.AddScoped<IFeatureEvaluationObserver, HttpContextItemObserver>();

            return builder;
        }
    }
}

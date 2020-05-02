using Esquio.AspNetCore.ApplicationInsightProcessor.Processor;
using Esquio.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class EsquioBuilderExtensions
    {
        /// <summary>
        /// Add Application Insight observer that include Esquio feature evaluation
        /// results into all <see cref="Microsoft.ApplicationInsights.Channel.ITelemetry"/> 
        /// entries sent to Application Insight.
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <returns>>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        public static IEsquioBuilder AddApplicationInsightProcessor(this IEsquioBuilder builder)
        {
            builder.Services
                .AddApplicationInsightsTelemetry()
                .AddApplicationInsightsTelemetryProcessor<EsquioAspNetScopedEvaluationSessionProcessor>();

            return builder;
        }
    }
}

using Esquio.Abstractions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Processor
{
    public sealed class EsquioAspNetScopedEvaluationSessionProcessor
        : ITelemetryProcessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITelemetryProcessor _next;

        public EsquioAspNetScopedEvaluationSessionProcessor(ITelemetryProcessor next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Process(ITelemetry item)
        {
            AddEsquioProperties(item);

            if (_next != null)
            {
                _next.Process(item);
            }
        }

        private void AddEsquioProperties(ITelemetry telemetryItem)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var esquioScopedEvaluationResults = _httpContextAccessor.HttpContext?
                    .Items
                    .Where(i => i.Key.ToString().StartsWith(EsquioConstants.ESQUIO));

                if (esquioScopedEvaluationResults != null)
                {
                    ISupportProperties telemetry = telemetryItem as ISupportProperties;

                    if (telemetry != null
                        &&
                        esquioScopedEvaluationResults != null)
                    {
                        foreach (var (key, value) in esquioScopedEvaluationResults)
                        {
                            if (!telemetry.Properties.ContainsKey(key.ToString()))
                            {
                                telemetry.Properties.Add(key.ToString(), ((ScopedEvaluationResult)value).Enabled.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}

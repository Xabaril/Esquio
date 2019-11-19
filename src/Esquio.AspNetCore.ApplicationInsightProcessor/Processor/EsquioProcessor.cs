using Esquio.Abstractions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Processor
{
    public sealed class EsquioProcessor
        : ITelemetryProcessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITelemetryProcessor _next;

        public EsquioProcessor(ITelemetryProcessor next, IHttpContextAccessor httpContextAccessor)
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
                const string ITEMS_PREFIX = "Esquio";

                var esquioScopedEvaluationResults = _httpContextAccessor.HttpContext?
                    .Items
                    .Where(i => i.Key.ToString().StartsWith(ITEMS_PREFIX));

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

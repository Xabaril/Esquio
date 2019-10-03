using Esquio.AspNetCore.ApplicationInsightProcessor.Diagnostics;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Processor
{
    internal class EsquioProcessor
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
            var esquioContextItems = _httpContextAccessor.HttpContext?
                .Items
                .Where(item => item.Key.ToString().StartsWith(HttpContextItemObserver.EsquioItemKeyName));

            ISupportProperties telemetry = telemetryItem as ISupportProperties;

            if (telemetry != null
                &&
                esquioContextItems != null)
            {
                foreach (var (key, value) in esquioContextItems)
                {
                    if (!telemetry.Properties.ContainsKey(key.ToString()))
                    {
                        telemetry.Properties.Add(key.ToString(), value.ToString());
                    }
                }
            }
        }
    }
}

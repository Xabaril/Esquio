using Esquio.Abstractions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Processor
{
    public sealed class EsquioProcessor
        : ITelemetryProcessor
    {
        const string KEY_PREFIX = "Esquio";

        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITelemetryProcessor _next;

        public EsquioProcessor(ITelemetryProcessor next/*, IHttpContextAccessor httpContextAccessor*/)
        {
            _next = next;
            //_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Process(ITelemetry item)
        {
            //AddEsquioPropertiesAsync(item);

            //if (_next != null)
            //{
                _next.Process(item);
            //}
        }

        //private void AddEsquioPropertiesAsync(ITelemetry telemetryItem)
        //{
        //    if (_httpContextAccessor.HttpContext != null)
        //    {
        //        var session = _httpContextAccessor.HttpContext.RequestServices
        //          .GetService<IEvaluationSession>();

        //        if (session != null)
        //        {
        //            var entries = session.GetAllAsync().Result;

        //            ISupportProperties telemetry = telemetryItem as ISupportProperties;

        //            if (telemetry != null
        //                &&
        //                entries != null)
        //            {
        //                foreach (var item in entries)
        //                {
        //                    var key = $"{KEY_PREFIX}:{item.ProductName ?? EsquioConstants.DEFAULT_PRODUCT_NAME}:{item.FeatureName}";

        //                    if (!telemetry.Properties.ContainsKey(key.ToString()))
        //                    {
        //                        telemetry.Properties.Add(key.ToString(), item.Enabled.ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}

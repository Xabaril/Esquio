using Esquio.Abstractions;
using Esquio.AspNetCore.ApplicationInsightProcessor.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.ApplicationInsightProcessor
{
    internal class HttpContextItemObserver
        : IFeatureEvaluationObserver
    {
        internal const string EsquioItemKeyName = "Esquio";

        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly ILogger<HttpContextItemObserver> _logger;

        public HttpContextItemObserver(IHttpContextAccessor httpContextAccesor, ILogger<HttpContextItemObserver> logger)
        {
            _httpContextAccesor = httpContextAccesor ?? throw new ArgumentNullException(nameof(httpContextAccesor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task OnNext(string featureName, string productName = null, bool enabled = false, CancellationToken cancellationToken = default)
        {
            var currentContext = _httpContextAccesor.HttpContext;

            if (currentContext != null)
            {
                var key = $"{EsquioItemKeyName}:{productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME}:{featureName}";

                if (!currentContext.Items.ContainsKey(key))
                {
                    if (!currentContext.Items.TryAdd(key, enabled))
                    {
                        Log.HttpContextItemObserverCantAddItem(_logger, key);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}

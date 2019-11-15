using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    internal class HttpContextEvaluationSession
        : IEvaluationSession
    {
        const string KEY_PREFIX = "Esquio";
        const string KEY_FORMAT = "{0}:{1}:{2}";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextEvaluationSession(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccessor = httpContextAccesor ?? throw new ArgumentNullException(nameof(httpContextAccesor));
        }

        public Task SetAsync(string featureName, string productName, bool enabled)
        {
            var key = GetKey(featureName, productName);

            var added = _httpContextAccessor.HttpContext
                .Items
                .TryAdd(key, new EvaluationResult()
                {
                    FeatureName = featureName,
                    ProductName = productName,
                    Enabled = enabled
                });

            return Task.CompletedTask;
        }

        public Task<bool> TryGetAsync(string featureName, string productName, out bool enabled)
        {
            var key = GetKey(featureName, productName);

            if (_httpContextAccessor.HttpContext.Items
                .TryGetValue(key, out var value))
            {
                enabled = ((EvaluationResult)value).Enabled;
                return Task.FromResult(true);
            }

            enabled = false;
            return Task.FromResult(false);
        }

        string GetKey(string featureName, string productName)
        {
            return string.Format(KEY_FORMAT, KEY_PREFIX, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, featureName);
        }
    }
}

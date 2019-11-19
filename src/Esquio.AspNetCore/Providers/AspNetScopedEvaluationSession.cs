using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    internal sealed class AspNetScopedEvaluationSession
        : IScopedEvaluationSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetScopedEvaluationSession(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccessor = httpContextAccesor ?? throw new ArgumentNullException(nameof(httpContextAccesor));
        }

        public Task SetAsync(string featureName, string productName, bool enabled)
        {
            var key = GetKey(featureName, productName);

            _httpContextAccessor.HttpContext
                .Items
                .TryAdd(key, new ScopedEvaluationResult()
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

            if (_httpContextAccessor.HttpContext
                .Items
                .TryGetValue(key, out var value))
            {
                enabled = ((ScopedEvaluationResult)value).Enabled;
                return Task.FromResult(true);
            }

            enabled = false;
            return Task.FromResult(false);
        }

        string GetKey(string featureName, string productName)
        {
            return $"{EsquioConstants.ESQUIO}:{productName}:{featureName}";
        }
    }
}

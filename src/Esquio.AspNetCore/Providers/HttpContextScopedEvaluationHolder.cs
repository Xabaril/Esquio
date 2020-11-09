using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    internal sealed class HttpContextScopedEvaluationHolder
        : IScopedEvaluationHolder
    {
        private readonly ConcurrentDictionary<string, bool> _evaluationResults = new ConcurrentDictionary<string, bool>();

        private readonly IHttpContextAccessor _httpContextAccesor;

        public HttpContextScopedEvaluationHolder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccesor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            _httpContextAccesor.HttpContext?
                .Items
                .Add(EsquioConstants.ESQUIO, _evaluationResults);
        }

        public Task SetAsync(string featureName, bool enabled)
        {
            _evaluationResults.TryAdd(featureName, enabled);
            return Task.CompletedTask;
        }

        public Task<bool> TryGetAsync(string featureName, out bool enabled)
        {
            return Task.FromResult(_evaluationResults.TryGetValue(featureName, out enabled));
        }
    }
}
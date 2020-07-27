using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Providers
{
    internal sealed class HttpContextScopedEvaluationHolder
        : IScopedEvaluationHolder
    {
        private readonly Dictionary<string, bool> _evaluationResults = new Dictionary<string, bool>();

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
            if ( !_evaluationResults.ContainsKey(featureName))
            {
                _evaluationResults.Add(featureName, enabled);
            }

            return Task.CompletedTask;
        }

        public Task<bool> TryGetAsync(string featureName, out bool enabled)
        {
            enabled = false;

            if ( _evaluationResults.ContainsKey(featureName))
            {
                enabled = _evaluationResults[featureName];
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}

using Esquio.Blazor.WebAssembly.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Esquio.Blazor.WebAssembly
{
    public interface IBlazorFeatureServiceClient
    {
        Task<bool> IsEnabledAsync(string featureName);
    }

    public class BlazorFeatureServiceClient
        : IBlazorFeatureServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<BlazorFeatureServiceOptions> _options;
        private readonly ILogger<BlazorFeatureServiceClient> _logger;

        public BlazorFeatureServiceClient(HttpClient httpClient, IOptions<BlazorFeatureServiceOptions> options, ILogger<BlazorFeatureServiceClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> IsEnabledAsync(string featureName)
        {
            try
            {
                var endpoint = _options?.Value.Endpoint ?? "Esquio";

                var response = await _httpClient
                    .GetFromJsonAsync<IEnumerable<EvaluationResponse>>($"{endpoint}?featureName={featureName}");

                if (response != null && response.Any())
                {
                    var evaluation = response
                        .Where(f => f.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase))
                        .SingleOrDefault();

                    if ( evaluation != null)
                    {
                        return evaluation.Enabled;
                    }
                }

                return false;
            }
            catch (Exception exception)
            {
                _logger.LogError("Blazor feature service client throw with error", exception);
                return false;
            }
        }

        private class EvaluationResponse
        {
            public bool Enabled { get; set; }

            public string Name { get; set; }
        }
    }
}

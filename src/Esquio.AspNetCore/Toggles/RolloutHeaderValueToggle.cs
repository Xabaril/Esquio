using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignType(Description = "Toggle that is active depending on the bucket name created with current request header name/value pair.")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = "System.Int32", ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    [DesignTypeParameter(ParameterName = HeaderName, ParameterType = "System.String", ParameterDescription = "The name of the header to get the value and create partitions buckets over this.")]
    public class RolloutHeaderValueToggle
        : IToggle
    {
        internal const string HeaderName = nameof(HeaderName);
        internal const string Percentage = nameof(Percentage);
        internal const int Partitions = 10;

        private readonly IRuntimeFeatureStore _featureStore;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RolloutHeaderValueToggle(IRuntimeFeatureStore featureStore, IHttpContextAccessor httpContextAccessor)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            string headerName = data.HeaderName;
            int percentage = data.Percentage;

            var values = _httpContextAccessor.HttpContext.Request.Headers[headerName];
            var headerValue = values != StringValues.Empty ? values.First() : "NoHeaderValue";
            var assignedPartition = Partitioner.ResolveToLogicalPartition(headerValue, Partitions);

            return assignedPartition <= ((Partitions * percentage) / 100);
        }
    }
}

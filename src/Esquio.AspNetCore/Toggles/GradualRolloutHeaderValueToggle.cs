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
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = EsquioConstants.PERCENTAGE_PARAMETER_TYPE, ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    [DesignTypeParameter(ParameterName = HeaderName, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The name of the header to get the value and create partitions buckets over this.")]
    public class GradualRolloutHeaderValueToggle
        : IToggle
    {
        const string NO_HEADER_DEFAULT_VALUE = "no-header-value";

        internal const string HeaderName = nameof(HeaderName);
        internal const string Percentage = nameof(Percentage);

        private readonly IValuePartitioner _partitioner;
        private readonly IRuntimeFeatureStore _featureStore;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GradualRolloutHeaderValueToggle(IValuePartitioner partitioner,
            IRuntimeFeatureStore featureStore, 
            IHttpContextAccessor httpContextAccessor)
        {
            _partitioner = partitioner ?? throw new ArgumentNullException(nameof(partitioner));
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName, cancellationToken);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            string headerName = data.HeaderName;
            if (Double.TryParse(data.Percentage.ToString(), out double percentage))
            {
                if (percentage > 0d)
                {
                    var values = _httpContextAccessor.HttpContext
                        .Request
                        .Headers[headerName];

                    var headerValue = values != StringValues.Empty ? values.First() : NO_HEADER_DEFAULT_VALUE;

                    var assignedPartition = _partitioner.ResolvePartition(headerValue);
                    return assignedPartition <= percentage;
                }
            }

            return false;
        }
    }
}

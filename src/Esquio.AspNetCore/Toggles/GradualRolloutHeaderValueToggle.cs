using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on current header value specified on HeaderName property and how this value is assigned to a specific partition using the 
    /// configured <see cref="IValuePartitioner"/>. This <see cref="IToggle"/> create 100 buckets for partitioner and assign the header vaulue into a specific
    /// bucket. If assigned bucket is less or equal that Percentage property value this toggle is active.
    /// </summary>
    [DesignType(Description = "The request header exists and its value falls inside the percentage name.", FriendlyName = "Gradual rollout by Http Header value")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = EsquioConstants.PERCENTAGE_PARAMETER_TYPE, ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    [DesignTypeParameter(ParameterName = HeaderName, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The name of the header to get the value and create partitions buckets over this.")]
    public class GradualRolloutHeaderValueToggle
        : IToggle
    {
        internal const string HeaderName = nameof(HeaderName);
        internal const string Percentage = nameof(Percentage);

        private readonly IValuePartitioner _partitioner;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="partitioner">The <see cref="IValuePartitioner"/> service to be used.</param>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> service to be used.</param>
        public GradualRolloutHeaderValueToggle(
            IValuePartitioner partitioner,
            IHttpContextAccessor httpContextAccessor)
        {
            _partitioner = partitioner ?? throw new ArgumentNullException(nameof(partitioner));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc/>
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            string headerName = context.Data[HeaderName].ToString();

            if (Double.TryParse(context.Data[Percentage].ToString(), out double percentage))
            {
                if (percentage > 0d)
                {
                    var values = _httpContextAccessor.HttpContext
                        .Request
                        .Headers[headerName];

                    if (values != StringValues.Empty)
                    {
                        // this only apply when header exist, we apply also some entropy to header value.
                        // adding this entropy ensure that not all features with gradual rollout for claim value are enabled/disable at the same time for the same user.

                        var assignedPartition = _partitioner.ResolvePartition(context.FeatureName + values.First(), partitions: 100);
                        var active = assignedPartition <= percentage;

                        return new ValueTask<bool>(active);
                    }
                }
            }

            return new ValueTask<bool>(false);
        }
    }
}

using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on the current User name and how this name is assigned to a specific partition using the 
    /// configured <see cref="IValuePartitioner"/>. This <see cref="IToggle"/> create 100 buckets for partitioner and assign the current user name into a specific
    /// bucket. If assigned bucket is less or equal that Percentage property value this toggle is active.
    /// </summary>
    [DesignType(Description = "The current user name falls within the percentage.", FriendlyName = "Gradual rollout by UserName")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = EsquioConstants.PERCENTAGE_PARAMETER_TYPE, ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    public class GradualRolloutUserNameToggle
        : IToggle
    {
        internal const string Percentage = nameof(Percentage);
        internal const int Partitions = 100;

        private readonly IValuePartitioner _partitioner;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Create a new instance of <see cref="GradualRolloutUserNameToggle"/> toggle.
        /// </summary>
        /// <param name="partitioner">The <see cref="IValuePartitioner"/> service to be used.</param>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> service to be used.</param>
        public GradualRolloutUserNameToggle(IValuePartitioner partitioner, IHttpContextAccessor httpContextAccessor)
        {
            _partitioner = partitioner ?? throw new ArgumentNullException(nameof(partitioner));
            _httpContextAccessor = httpContextAccessor ?? throw new System.ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc/>
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            if (Double.TryParse(context.Data[Percentage].ToString(), out double percentage))
            {
                if (percentage > 0)
                {
                    var currentUserName = GetCurrentUserName();

                    if (currentUserName != null)
                    {
                        // this only apply for authenticted users, we apply some entropy to currentUserName.
                        // adding this entropy ensure that not all features with gradual rollout for username are enabled/disable at the same time for the same user.

                        var assignedPartition = _partitioner.ResolvePartition(context.FeatureName + currentUserName, partitions: 100);

                        return new ValueTask<bool>(assignedPartition <= percentage);
                    }
                }
            }

            return new ValueTask<bool>(false);
        }

        private string GetCurrentUserName()
        {
            //TODO: buscarlo por la claim seleccionada 

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var userName = httpContext
                    .User?
                    .Identity?
                    .Name;

                return userName;
            }

            throw new InvalidOperationException($"HttpContext is null and {nameof(UserNameToggle)} can't recover the current User name for this provider.");
        }
    }
}

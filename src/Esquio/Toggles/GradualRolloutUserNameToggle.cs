using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{

    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on the current User name and how this name is assigned to a specific partition using the 
    /// configured <see cref="IValuePartitioner"/>. This <see cref="IToggle"/> create 100 buckets for partitioner and assign the current user name into a specific
    /// bucket. If assigned bucket is less or equal that Percentage property value this toggle is active.
    /// </summary>
    [DesignType(Description = "Toggle that is active depending on the bucket name created with user name value and the rollout percentage.", FriendlyName = "Gradual rollout by UserName")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = EsquioConstants.PERCENTAGE_PARAMETER_TYPE, ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    public class GradualRolloutUserNameToggle
        : IToggle
    {
        internal const string Percentage = nameof(Percentage);
        internal const int Partitions = 100;

        private readonly IUserNameProviderService _userNameProviderService;
        private readonly IValuePartitioner _partitioner;
        private readonly IRuntimeFeatureStore _featureStore;

        /// <summary>
        /// Create a new instance of <see cref="GradualRolloutUserNameToggle"/> toggle.
        /// </summary>
        /// <param name="partitioner">The <see cref="IValuePartitioner"/> service to be used.</param>
        /// <param name="userNameProviderService">The <see cref="IUserNameProviderService"/> service to be used.</param>
        /// <param name="featureStore">The <see cref="IRuntimeFeatureStore"/> service to be used.</param>
        public GradualRolloutUserNameToggle(
            IValuePartitioner partitioner,
            IUserNameProviderService userNameProviderService,
            IRuntimeFeatureStore featureStore)
        {
            _partitioner = partitioner ?? throw new ArgumentNullException(nameof(partitioner));
            _userNameProviderService = userNameProviderService ?? throw new System.ArgumentNullException(nameof(userNameProviderService));
            _featureStore = featureStore ?? throw new System.ArgumentNullException(nameof(featureStore));
        }

        /// <inheritdoc/>
        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore
                .FindFeatureAsync(featureName, productName, cancellationToken);

            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            if (Double.TryParse(data.Percentage.ToString(), out double percentage))
            {
                if (percentage > 0)
                {
                    var currentUserName = await _userNameProviderService
                        .GetCurrentUserNameAsync();

                    if (currentUserName != null)
                    {
                        // this only apply for authenticted users, we apply some entropy to currentUserName.
                        // adding this entropy ensure that not all features with gradual rollout for username are enabled/disable at the same time for the same user.

                        var assignedPartition = _partitioner.ResolvePartition(featureName + currentUserName, partitions: 100);

                        return assignedPartition <= percentage;
                    }
                }
            }

            return false;
        }
    }
}


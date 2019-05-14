using System;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Provides extensions  methods for <see cref="IFeatureService"/>.
    /// </summary>
    public static class FeatureServiceExtensions
    {
        /// <summary>
        /// Configure actions to be executed depending on the feature active state.
        /// </summary>
        /// <param name="featureService">The <see cref="IFeatureService"/>.</param>
        /// <param name="featureName">The feature name to be evaluated.</param>
        /// <param name="active">Action to be executed when <paramref name="featureName"/> is active. </param>
        /// <param name="notActive">Action to be executed when <paramref name="featureName"/> is not active.</param>
        /// <param name="productName">The product name.</param>
        /// <returns>A <see cref="Task"/> that complete when <paramref name="active"/> or <paramref name="notActive"/> is executed.</returns>
        public static async Task Do(this IFeatureService featureService, string featureName, Action active, Action notActive, string productName = null)
        {
            if (await featureService.IsEnabledAsync(featureName, productName))
            {
                active();
            }
            else
            {
                notActive();
            }
        }

        /// <summary>
        /// Configure actions to be executed depending on the feature active state.
        /// </summary>
        /// <typeparam name="TResult">The action return type.</typeparam>
        /// <param name="featureService">The <see cref="IFeatureService"/>.</param>
        /// <param name="featureName">The feature name to be evaluated.</param>
        /// <param name="active">Action to be executed when <paramref name="featureName"/> is active. </param>
        /// <param name="notActive">Action to be executed when <paramref name="featureName"/> is not active.</param>
        /// <param name="productName">The product name.</param>
        /// <returns>A <see cref="Task{TResult}"/> that complete when <paramref name="active"/> or <paramref name="notActive"/> is executed.</returns>
        public static async Task<TResult> Do<TResult>(this IFeatureService featureService, string featureName, Func<TResult> active, Func<TResult> notActive, string productName = null)
        {
            if (await featureService.IsEnabledAsync(featureName, productName))
            {
                return active();
            }
            else
            {
                return notActive();
            }
        }
    }
}

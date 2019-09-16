using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent the base contract for <see cref="Esquio.Abstractions.IFeatureObserver"/>.
    /// </summary>
    public interface IFeatureObserver
    {
        /// <summary>
        /// Notify a feature execution.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <param name="productName">The product name where feature is defined.</param>
        /// <param name="enabled">The evaluation result of the feature.</param>
        /// <param name="cancellationToken"> A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that complete when operation finished.</returns>
        Task OnNext(string featureName, string productName = default, bool enabled = default, CancellationToken cancellationToken = default);
    }

    class NoFeatureObserver
        : IFeatureObserver
    {
        public Task OnNext(string featureName, string productName = null, bool enabled = false, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}

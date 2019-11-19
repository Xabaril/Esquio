using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Base contract for <see cref="IScopedEvaluationSession"/> used for store feature evaluation session results on the same execution scope.
    /// By default, a NO evaluation session is used and scoped evaluation results are never stored and reused on <see cref="IFeatureService"/>.
    /// </summary>
    public interface IScopedEvaluationSession
    {
        /// <summary>
        /// Try to get a previous feature evaluation from the session store.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <param name="productName">The product name.</param>
        /// <param name="enabled">The feature and product evaluation result.</param>
        /// <returns>A <see cref="Task{bool}"/> that complete when finished, yielding True if session contain a previous evaluation result, else False.</returns>
        Task<bool> TryGetAsync(string featureName, string productName, out bool enabled);

        /// <summary>
        /// Set evaluation result into this session store.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <param name="productName">The product name.</param>
        /// <returns>A <see cref="Task"/> that complete when finished.</returns>
        Task SetAsync(string featureName, string productName, bool enabled);
    }

    public class ScopedEvaluationResult
    {
        public bool Enabled { get; set; }

        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }

    internal sealed class NoScopedEvaluationSession
        : IScopedEvaluationSession
    {
        public Task SetAsync(string featureName, string productName, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task<bool> TryGetAsync(string featureName, string productName, out bool enabled)
        {
            enabled = false;
            return Task.FromResult(false);
        }
    }
}

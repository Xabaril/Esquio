using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Base contract for <see cref="IScopedEvaluationHolder"/> used for store feature evaluation results on the same execution scope.
    /// This reduce the numbers of evaluations and ensure result consistency on the same scope (http request on asp.net core etc).
    /// By default, a NO evaluation holder is used and scoped evaluation results are never stored and reused on <see cref="IFeatureService"/>.
    /// </summary>
    public interface IScopedEvaluationHolder
    {
        /// <summary>
        /// Try to get a previous feature evaluation from the session store.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <param name="enabled">The feature and product evaluation result.</param>
        /// <returns>A <see cref="Task{bool}"/> that complete when finished, yielding True if session contain a previous evaluation result, else False.</returns>
        Task<bool> TryGetAsync(string featureName, out bool enabled);

        /// <summary>
        /// Set evaluation result into this session store.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <returns>A <see cref="Task"/> that complete when finished.</returns>
        Task SetAsync(string featureName, bool enabled);
    }

    internal sealed class NoScopedEvaluationHolder
        : IScopedEvaluationHolder
    {
        public Task SetAsync(string featureName, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task<bool> TryGetAsync(string featureName, out bool enabled)
        {
            enabled = false;
            return Task.FromResult(false);
        }
    }
}

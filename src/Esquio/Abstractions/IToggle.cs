using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent the base contract for Toggles.
    /// </summary>
    public interface IToggle
    {
        /// <summary>
        /// Check if the toggle is active for an application and feature.
        /// </summary>
        /// <param name="context">The toogle execution context.</param>
        /// <param name="cancellationToken"> A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="ValueTask{bool}"/> that complete when finished, yielding True if the toggle is active, else false.</returns>
        ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default);
    }
}

using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent the base contract for all Toggles.
    /// </summary>
    public interface IToggle
    {
        /// <summary>
        /// Check if the toggle is active on specified feature context.
        /// </summary>
        /// <param name="context"><see cref="Esquio.Abstractions.IFeatureContextFactory"/>.</param>
        /// <returns>True if context is active, else False.</returns>
        Task<bool> IsActiveAsync(IFeatureContext context);
    }
}

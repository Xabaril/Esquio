namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent the contract for Toggle type activators.
    /// </summary>
    public interface IToggleTypeActivator
    {
        /// <summary>
        /// Create a new instance of the type <paramref name="toggle"/>.
        /// </summary>
        /// <param name="toggleTypeName">The type of the toggle to create.</param>
        /// <returns>A new instance of the type <paramref name="toggle"/>.</returns>
        IToggle CreateInstance(string toggleTypeName);
    }
}

namespace Esquio.Abstractions
{
    /// <summary>
    /// Represent the base contract for <see cref="Esquio.Abstractions.IToggle"/> activator.
    /// </summary>
    public interface IToggleTypeActivator
    {
        /// <summary>
        /// Service that create new instances of <see cref="Esquio.Abstractions.IToggle"/>  using the type specified on <paramref name="toggle"/>.
        /// </summary>
        /// <param name="toggleTypeName">The type of the toggle to be created.</param>
        /// <returns>A new instance of the type <paramref name="toggle"/> or null if instance can't be created.</returns>
        IToggle CreateInstance(string toggleTypeName);
    }
}

namespace Esquio.DependencyInjection
{
    /// <summary>
    /// Define on error behavior options.
    /// </summary>
    public enum OnErrorBehavior : short
    {
        /// <summary>
        /// Re-throw the exception throwed when feature is evaluated.
        /// </summary>
        Throw = 0,

        /// <summary>
        /// Set feature as disabled.
        /// </summary>
        SetDisabled = 1,

        /// <summary>
        /// Set feature as enabled
        /// </summary>
        SetEnabled = 2
    }
    /// <summary>
    /// Define not found behavior options.
    /// </summary>
    public enum NotFoundBehavior : short
    {
        /// <summary>
        /// Set feature as disabled.
        /// </summary>
        SetDisabled = 1,

        /// <summary>
        /// Set feature as enabled.
        /// </summary>
        SetEnabled = 2
    }
}

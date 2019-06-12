using System;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Allow add design time description for <see cref="Esquio.Abstractions.IToggle"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DesignTypeAttribute
        : Attribute
    {
        /// <summary>
        /// Get or set the toggle description to  be used on design time.
        /// </summary>
        public string Description { get; set; }
    }
}

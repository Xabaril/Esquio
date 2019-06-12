using System;

namespace Esquio.Abstractions
{
    /// <summary>
    /// Allow add design time parameters information of <see cref="Esquio.Abstractions.IToggle"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DesignTypeParameterAttribute
        : Attribute
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// The CLR FQN parameter type.
        /// </summary>
        public string ParameterType { get; set; }
        /// <summary>
        /// The parameter description.
        /// </summary>
        public string ParameterDescription { get; set; }
    }
}

using System;

namespace Esquio.Abstractions
{
    /// <summary>
    /// The attribute used to define design time parameters of author Toggles.
    /// This is used by UI's to discover the parameters that Toggle need to work.
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

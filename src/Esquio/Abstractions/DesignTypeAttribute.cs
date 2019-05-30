using System;

namespace Esquio.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DesignTypeAttribute
        : Attribute
    {
        /// <summary>
        /// The toggle description
        /// </summary>
        public string Description { get; set; }
    }
}

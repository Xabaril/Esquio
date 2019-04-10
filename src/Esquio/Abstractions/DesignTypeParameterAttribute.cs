using System;

namespace Esquio.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DesignTypeParameterAttribute
        : Attribute
    {
        public string ParameterName { get; set; }
        public string ParameterType { get; set; }
        public string ParameterDescription { get; set; }
    }
}

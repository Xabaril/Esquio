using System;

namespace Esquio.Configuration.Store
{
    public class EsquioConfigurationException
        : Exception
    {
        public EsquioConfigurationException(string message, Exception innerException) : base(message, innerException) { }
        public EsquioConfigurationException(string message) : base(message) { }
    }
}

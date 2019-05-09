using System;

namespace Esquio
{
    public class EsquioException
        : Exception
    {
        public EsquioException(string message, Exception innerException) : base(message, innerException) { }
        public EsquioException(string message) : base(message) { }
    }
}

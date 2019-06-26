using Microsoft.AspNetCore.Http;
using System;

namespace Esquio.AspNetCore.Endpoints
{
    internal class EndpointFallbackService
    {
        public RequestDelegate RequestDelegate { get; private set; }

        public string EndpointDisplayName { get; private set; }

        public EndpointFallbackService(RequestDelegate requestDelegate, string endpointDisplayName = nameof(EndpointFallbackService))
        {
            RequestDelegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
            EndpointDisplayName = endpointDisplayName;
        }
    }
}

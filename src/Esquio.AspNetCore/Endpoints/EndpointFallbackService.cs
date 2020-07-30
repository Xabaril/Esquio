using Microsoft.AspNetCore.Http;
using System;

namespace Esquio.AspNetCore.Endpoints
{
    internal class EndpointFallbackService
    {
        public Func<PathString, RequestDelegate> DefaultRequestDelegateCreator { get; private set; }

        public EndpointFallbackService(Func<PathString, RequestDelegate> defaultRequestDelegateCreator)
        {
            DefaultRequestDelegateCreator = defaultRequestDelegateCreator ?? throw new ArgumentNullException(nameof(defaultRequestDelegateCreator));
        }

        public Endpoint CreateFallbackEndpoint(PathString path)
        {
            var requestDelegate = DefaultRequestDelegateCreator(path);

            return new Endpoint(requestDelegate, EndpointMetadataCollection.Empty, displayName: "EsquioFallbackEndpoint");
        }
    }
}

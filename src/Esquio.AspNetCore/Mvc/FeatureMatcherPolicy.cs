using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Mvc
{
    public class FeatureMatcherPolicy
        : MatcherPolicy, IEndpointSelectorPolicy
    {
        /// <inheritdoc />
        public override int Order => 0;

        public FeatureMatcherPolicy()
        {
        }

        /// <inheritdoc />
        public bool AppliesToEndpoints(IReadOnlyList<Microsoft.AspNetCore.Http.Endpoint> endpoints)
        {
            for (int i = 0; i < endpoints.Count; i++)
            {
                var metadata = endpoints[i].Metadata;
            }

            return false;
        }

        /// <inheritdoc />
        public Task ApplyAsync(HttpContext httpContext, EndpointSelectorContext context, CandidateSet candidates)
        {
            return Task.CompletedTask;
        }
    }
}

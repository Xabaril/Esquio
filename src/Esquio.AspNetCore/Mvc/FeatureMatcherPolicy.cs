using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Mvc
{
    /// <summary>
    /// TODO: Pending Preview 6 to apply this enpoint selector policy
    /// </summary>
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
            

            return false;
        }

        /// <inheritdoc />
        public Task ApplyAsync(HttpContext httpContext, EndpointSelectorContext context, CandidateSet candidates)
        {
            return Task.CompletedTask;
        }
    }
}

using Esquio.AspNetCore.Endpoint.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoint
{

    public class FeatureMatcherPolicy
        : MatcherPolicy, IEndpointSelectorPolicy
    {
        /// <inheritdoc />
        public override int Order => 0;

        public bool AppliesToEndpoints(IReadOnlyList<Microsoft.AspNetCore.Http.Endpoint> endpoints)
        {
            foreach(var item in endpoints)
            {
                var metadata = item.Metadata.GetMetadata<FeatureData>();

                if ( metadata != null )
                {
                    return true;
                }
            }
            return false;
        }

        public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            return Task.CompletedTask;
        }
    }
}

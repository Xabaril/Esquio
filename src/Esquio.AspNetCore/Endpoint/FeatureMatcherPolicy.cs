using Esquio.Abstractions;
using Esquio.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoint
{
    internal class FeatureMatcherPolicy
        : MatcherPolicy, IEndpointSelectorPolicy
    {
        public override int Order => Int32.MaxValue;

        public bool AppliesToEndpoints(IReadOnlyList<Microsoft.AspNetCore.Http.Endpoint> endpoints)
        {
            var apply = false;
            foreach (var item in endpoints)
            {
                var metadata = item.Metadata.GetMetadata<FeatureFilter>();

                if (metadata != null)
                {
                    apply = true;
                    break;
                }
            }

            return apply;
        }

        public async Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            var featureService = httpContext.RequestServices
                .GetService<IFeatureService>();

            for (int index = 0; index < candidates.Count; index++)
            {
                var endpoint = candidates[index].Endpoint;

                var metadata = endpoint?.Metadata
                    .GetMetadata<FeatureFilter>();

                if (metadata != null)
                {
                    var enabled = await featureService
                        .IsEnabledAsync(metadata.Names, metadata.Product);
                    
                    candidates.SetValidity(index, enabled);
                }
            }
        }
    }
}

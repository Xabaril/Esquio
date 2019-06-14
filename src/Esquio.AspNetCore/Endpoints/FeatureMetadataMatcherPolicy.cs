using Esquio.Abstractions;
using Esquio.AspNetCore.Endpoints.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoints
{
    internal class FeatureMetadataMatcherPolicy
        : MatcherPolicy, IEndpointSelectorPolicy
    {
        static char[] split_characters = new char[] { ',' };

        public override int Order => Int32.MaxValue;

        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            var apply = false;
            foreach (var item in endpoints)
            {
                var metadata = item.Metadata
                    .GetMetadata<FeatureFilterMetadata>();

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
            for (int index = 0; index < candidates.Count; index++)
            {
                var endpoint = candidates[index].Endpoint;

                var metadata = endpoint?.Metadata
                   .GetMetadata<FeatureFilterMetadata>();

                if (metadata != null)
                {
                    var featureService = httpContext
                        .RequestServices
                        .GetService<IFeatureService>();

                    var valid = true;

                    var tokenizer = new StringTokenizer(metadata.Names, split_characters);

                    foreach (var token in tokenizer)
                    {
                        var featureName = token.Trim();

                        if (featureName.HasValue && featureName.Length > 0)
                        {
                            if (!await featureService.IsEnabledAsync(featureName.Value, metadata.ProductName))
                            {
                                valid = false;
                                break;
                            }
                        }
                    }

                    candidates.SetValidity(index, valid);
                }
            }
        }
    }
}

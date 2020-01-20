using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Esquio.AspNetCore.Endpoints.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoints
{
    internal class FeatureMatcherPolicy
        : MatcherPolicy, IEndpointSelectorPolicy, IEndpointComparerPolicy
    {
        private static char[] split_characters = new char[] { ',' };

        private readonly EsquioAspNetCoreDiagnostics _diagnostics;

        public FeatureMatcherPolicy(EsquioAspNetCoreDiagnostics diagnostics)
        {
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        public override int Order => Int32.MaxValue;

        public IComparer<Endpoint> Comparer => EndpointMetadataComparer<FeatureFilter>.Default;

        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            var apply = false;

            foreach (var item in endpoints)
            {
                var metadata = item.Metadata
                    .GetMetadata<IFeatureFilterMetadata>();

                if (metadata != null)
                {
                    _diagnostics.FeatureMatcherPolicyCanBeAppliedToEndpoint(item.DisplayName);

                    apply = true;
                    break;
                }
            }

            return apply;
        }

        public async Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (candidates == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var hasCandidates = false;
            var valid = true;

            for (int index = 0; index < candidates.Count; index++)
            {
                var endpoint = candidates[index].Endpoint;

                var allMetadata = endpoint?.Metadata
                   .GetOrderedMetadata<IFeatureFilterMetadata>();

                if (allMetadata != null && allMetadata.Any())
                {
                    foreach (var metadata in allMetadata)
                    {
                        _diagnostics.FeatureMatcherPolicyEvaluatingFeatures(endpoint.DisplayName, metadata.Names);

                        var featureService = httpContext
                            .RequestServices
                            .GetService<IFeatureService>();

                        var tokenizer = new StringTokenizer(metadata.Names, split_characters);

                        foreach (var token in tokenizer)
                        {
                            var featureName = token.Trim();

                            if (featureName.HasValue && featureName.Length > 0)
                            {
                                if (!await featureService.IsEnabledAsync(featureName.Value))
                                {
                                    _diagnostics.FeatureMatcherPolicyEndpointIsNotValid(endpoint.DisplayName);

                                    valid = false & valid;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _diagnostics.FeatureMatcherPolicyEndpointIsValid(endpoint.DisplayName);

                    valid = true;
                }

                hasCandidates |= valid;
                candidates.SetValidity(index, value: valid);
            }

            if (!hasCandidates)
            {
                var fallbackService = httpContext
                      .RequestServices
                      .GetService<EndpointFallbackService>();

                if (fallbackService != null)
                {
                    _diagnostics.FeatureMatcherPolicyExecutingFallbackEndpoint(httpContext.Request.Path);

                    httpContext.SetEndpoint(
                        CreateFallbackEndpoint(fallbackService.RequestDelegate, fallbackService.EndpointDisplayName));

                    httpContext.Request.RouteValues = null;
                }
            }
        }

        private Endpoint CreateFallbackEndpoint(RequestDelegate requestDelegate, string displayName = "fallbackEndpoint")
        {
            return new Endpoint(requestDelegate, EndpointMetadataCollection.Empty, displayName);
        }
    }
}

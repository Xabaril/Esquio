using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.AspNetCore.Mvc
{
    public class FeatureFilterRouteConstraint
        : IRouteConstraint
    {
        private readonly string _featureName;
        private readonly string _productName;
        private readonly IServiceProvider _serviceProvider;

        public FeatureFilterRouteConstraint(IServiceProvider serviceProvider, string featureName, string productName = "")
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _featureName = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _productName = productName ?? string.Empty;
        }
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            using (var scope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var featureService = scope.ServiceProvider
                    .GetRequiredService<IFeatureService>();

                return featureService.IsEnabledAsync(_featureName, _productName)
                    .Result;
            }
        }
    }
}

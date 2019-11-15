using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Esquio.UI.Api.Infrastructure.Routes
{
    internal sealed class SlugRouteConstraint
        : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            _ = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _ = route ?? throw new ArgumentNullException(nameof(route));
            _ = route ?? throw new ArgumentNullException(nameof(routeKey));

            if (values.TryGetValue(routeKey, out var routeValue))
            {
                var valuetoMatch = Convert.ToString(routeValue, CultureInfo.InvariantCulture);

                Regex regex = new Regex(ApiConstants.Constraints.NamesRegularExpression,
                    options: RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
                    matchTimeout: TimeSpan.FromSeconds(1));

                return regex.IsMatch(valuetoMatch);
            }

            return false;
        }
    }
}

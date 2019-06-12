using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddIf(
            this IApplicationBuilder app,
            bool condition,
            Func<IApplicationBuilder, IApplicationBuilder> @if)
        {
            return condition ? @if(app) : app;
        }

        public static IApplicationBuilder AddIfElse(
            this IApplicationBuilder app,
            bool condition,
            Func<IApplicationBuilder, IApplicationBuilder> @if,
            Func<IApplicationBuilder, IApplicationBuilder> @else)
        {
            return condition ? @if(app) : @else(app);
        }
    }
}

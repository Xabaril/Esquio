using Esquio.Abstractions;
using Esquio.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EsquioBuilderExtensions
    {
        public static IEsquioBuilder AddMvcFallbackAction(this IEsquioBuilder builder, Func<ResourceExecutingContext, IActionResult> fallback)
        {
            builder.Services
                .AddSingleton<IMvcFallbackService>(sp =>
                {
                    return new DelegatedMvcFallbackService(fallback);
                });

            return builder;
        }
    }
}

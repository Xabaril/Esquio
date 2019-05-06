using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Esquio.AspNetCore.Mvc
{
    internal interface IMvcFallbackService
    {
        IActionResult GetFallbackActionResult(ResourceExecutingContext context);
    }
    internal class DelegatedMvcFallbackService
        :IMvcFallbackService
    {
        private readonly Func<ResourceExecutingContext, IActionResult> _fallback;

        public DelegatedMvcFallbackService(Func<ResourceExecutingContext, IActionResult> fallback)
        {
            _fallback = fallback ?? throw new ArgumentNullException(nameof(fallback));
        }

        public IActionResult GetFallbackActionResult(ResourceExecutingContext context)
        {
            return _fallback(context);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoints
{
    /// <summary>
    /// Define common fallback actions to be used when selected endpoint does not have candidates.
    /// </summary>
    public static class EndpointFallbackAction
    {
        /// <summary>
        /// Create a <see cref="Microsoft.AspNetCore.Http.RequestDelegate"/> with a redirect result to MVC action. 
        /// </summary>
        /// <param name="controllerName">The controller name used to create a redirect uri.</param>
        /// <param name="actionName">The action name used to create a redirect uri.</param>
        /// <returns>The <see cref="Microsoft.AspNetCore.Http.RequestDelegate"/> created with a redirect result.</returns>
        public static Func<PathString, RequestDelegate> RedirectAnyToAction(string controllerName, string actionName)
        {
            return (_) => new RequestDelegate(context =>
            {
                context.Response.Redirect($"{context.Request.Scheme}://{context.Request.Host}/{controllerName}/{actionName}");

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Create a <see cref="Microsoft.AspNetCore.Http.RequestDelegate"/> with a redirect result. 
        /// </summary>
        /// <param name="uri">The redirect uri to be used.</param>
        /// <returns>The <see cref="Microsoft.AspNetCore.Http.RequestDelegate"/> created with a redirect result.</returns>
        public static Func<PathString, RequestDelegate> RedirectAnyTo(string uri)
        {
            return (_) => new RequestDelegate(context =>
            {
                context.Response.Redirect(uri);

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Create a <see cref="Microsoft.AspNetCore.Http.RequestDelegate"/> with a NotFound status response. 
        /// </summary>
        /// <returns>The <see cref="Microsoft.AspNetCore.Http.RequestDelegate"/> created with NotFound response status.</returns>
        public static Func<PathString, RequestDelegate> NotFound()
        {
            return (_) => new RequestDelegate(context =>
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                return Task.CompletedTask;
            });
        }
    }
}

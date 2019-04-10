using Esquio.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
    public static class EsquioApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseXabaril(this IApplicationBuilder app, string path)
        {
            app.UseMiddleware<EsquioMiddleware>(path);

            return app;
        }
    }
}
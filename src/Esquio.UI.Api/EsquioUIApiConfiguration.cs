using Esquio.UI.Api.Features.Products.Add;
using Esquio.UI.Api.Infrastructure.Behaviors;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.UI.Api
{
    public static class EsquioUIApiConfiguration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(EsquioUIApiConfiguration))
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggerMediatRBehavior<,>))
                .AddCustomProblemDetails()
                .AddMvc()
                    .AddApplicationPart(typeof(EsquioUIApiConfiguration).Assembly)
                    .AddFluentValidation(setup => setup.RegisterValidatorsFromAssembly(typeof(AddProductValidator).Assembly))
                    .AddNewtonsoftJson()
                .Services;
        }

        public static IApplicationBuilder Configure(IApplicationBuilder app, Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)
                .UseProblemDetails()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}

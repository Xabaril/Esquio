using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Infrastructure.Behaviors;
using Esquio.UI.Api.Infrastructure.Metrics;
using Esquio.UI.Api.Infrastructure.Routes;
using Esquio.UI.Api.Infrastructure.Serialization;
using Esquio.UI.Api.Shared.Models.Products.Add;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.UI.Api
{
    public static class EsquioUIApiConfiguration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddAuthorization(setup =>
                {
                    setup.AddPolicy(Policies.Reader, builder => builder.AddRequirements(new PolicyRequirement(Policies.Reader)));
                    setup.AddPolicy(Policies.Contributor, builder => builder.AddRequirements(new PolicyRequirement(Policies.Contributor)));
                    setup.AddPolicy(Policies.Management, builder => builder.AddRequirements(new PolicyRequirement(Policies.Management)));
                })
                .AddScoped<IAuthorizationHandler, PolicyRequirementHandler>()
                .AddSingleton<EsquioMetricsClient>()
                .AddMediatR(typeof(EsquioUIApiConfiguration))
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggerMediatRBehavior<,>))
                .AddCustomProblemDetails()
                .AddApiVersioning(setup =>
                {
                    setup.DefaultApiVersion = new ApiVersion(3, 0);
                    setup.ReportApiVersions = true;
                    setup.AssumeDefaultVersionWhenUnspecified = true;
                    setup.UseApiBehavior = true;
                    setup.ApiVersionReader = ApiVersionReader.Combine(
                        new QueryStringApiVersionReader(),
                        new HeaderApiVersionReader(ApiConstants.ApiVersionHeaderName));
                })
                .AddVersionedApiExplorer()
                .AddMvc()
                    .AddApplicationPart(typeof(EsquioUIApiConfiguration).Assembly)
                    .AddFluentValidation(setup => 
                    {
                        setup.RegisterValidatorsFromAssembly(typeof(AddProductRequestValidator).Assembly);
                        setup.RegisterValidatorsFromAssembly(typeof(EsquioUIApiConfiguration).Assembly);
                    })
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new NumberToStringConverter());
                    })
                .Services
                .Configure<RouteOptions>(options =>
                {
                    options.ConstraintMap.Add("slug", typeof(SlugRouteConstraint));
                });

        }

        public static IApplicationBuilder Configure(IApplicationBuilder app,
            Func<IApplicationBuilder, IApplicationBuilder> preConfigure,
            Func<IApplicationBuilder, IApplicationBuilder> postConfigure)
        {
            var applicationBuilder = preConfigure(app)
                .UseProblemDetails()
                .UseRouting();

            return postConfigure(applicationBuilder)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapFallbackToFile("index.html");
                });
        }
    }
}

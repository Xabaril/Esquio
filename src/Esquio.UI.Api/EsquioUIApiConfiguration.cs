using Esquio.UI.Api.Features.Products.Add;
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
            services
                .AddMediatR(typeof(EsquioUIApiConfiguration))
                .AddCustomProblemDetails()
                //.AddMvc(options => options.EnableEndpointRouting = false)
                .AddMvc()
                .AddApplicationPart(typeof(EsquioUIApiConfiguration).Assembly)
                .AddFluentValidation(setup => setup.RegisterValidatorsFromAssembly(typeof(AddProductValidator).Assembly))
                .AddNewtonsoftJson();

            return services;
        }

        public static IApplicationBuilder Configure(IApplicationBuilder app, Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)
                .UseProblemDetails();
        }
    }
}

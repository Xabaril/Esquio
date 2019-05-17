using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Esquio.UI.Api
{
    public static class EsquioUIApiConfiguration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false)
              .AddFluentValidation(setup => setup.RegisterValidatorsFromAssembly(typeof(EsquioUIApiConfiguration).Assembly))
              .AddNewtonsoftJson();

            return services;
        }

        public static IApplicationBuilder Configure(IApplicationBuilder app, Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app);
        }
    }
}

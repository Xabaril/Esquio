using Esquio.UI.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Esquio.UI.Deployment.Setup
{
    public static class EsquioApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureEsquio(this
            IApplicationBuilder app, 
            IConfiguration configuration, 
            IWebHostEnvironment environment, 
            IApiVersionDescriptionProvider apiVersion)
        {
            EsquioUIApiConfiguration.Configure(app,
                preConfigure: appBuilder =>
                {
                    appBuilder.AddClientBlazorConfiguration();
                    appBuilder.UseResponseCompression();

                    if (environment.IsDevelopment())
                    {
                        appBuilder.UseDeveloperExceptionPage();
                        appBuilder.UseWebAssemblyDebugging();
                    }

                    return appBuilder
                       .UseCors(builder =>
                       {
                           var configuredOrigins = configuration.GetValue<string>("Cors:Origins");

                           if (!string.IsNullOrEmpty(configuredOrigins))
                           {
                               var allowedOrigins = configuredOrigins.Split(',');

                               builder.WithOrigins(allowedOrigins)
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                           }

                       }).UseDefaultFiles().UseStaticFiles();
                },
                postConfigure: appBuilder =>
                {
                    return appBuilder.UseAuthentication()
                    .UseAuthorization()
                    .UseBlazorFrameworkFiles()
                    .UseSwagger()
                    .UseSwaggerUI(setup =>
                    {
                        setup.EnableDeepLinking();
                        setup.OAuthScopeSeparator(" ");
                        setup.OAuthUsePkce();

                        foreach (var description in apiVersion.ApiVersionDescriptions)
                        {
                            setup.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                        }
                    });
                });

            return app;
        }
    }
}

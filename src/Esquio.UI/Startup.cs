using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Services;
using Esquio.UI.Infrastructure.OpenApi;
using Esquio.UI.Infrastructure.Security.ApiKey;
using Esquio.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Esquio.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddSingleton<IDiscoverToggleTypesService, DiscoverToggleTypesService>()
                .AddSwaggerGen()
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "secured";
                    options.DefaultChallengeScheme = "secured";
                })
                .AddApiKey()
                .AddJwtBearer(options =>
                {
                    Configuration.Bind("Security:Jwt", options);
                })
                .AddPolicyScheme("secured","Authorization Bearer or ApiKey",options=>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var bearer = context.Request.Headers["Authorization"].FirstOrDefault();

                        if (bearer != null && bearer.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }

                        return ApiKeyAuthenticationDefaults.ApiKeyScheme;
                    };
                });
                    

            EsquioUIApiConfiguration.ConfigureServices(services)
                .AddDbContext<StoreDbContext>(options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:Esquio"], setup =>
                     {
                         setup.MaxBatchSize(10);
                         setup.EnableRetryOnFailure();

                         setup.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                     });
                });
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider apiVersion)
        {
            EsquioUIApiConfiguration.Configure(app,
                preConfigure: host =>
                {
                    var rewriteOptions = new RewriteOptions()
                       .AddRewrite(@"^(?!.*(api\/|.*\.(js|css|ico)|fonts\/|img\/|static\/|swagger*|ws\/*)).*$", "index.html", skipRemainingRules: true);

                    return host
                        .UseCors(policy=>
                        {
                            policy
                                .AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        })
                        .UseSwagger()
                        .UseSwaggerUI(setup =>
                        {
                            foreach (var description in apiVersion.ApiVersionDescriptions)
                            {
                                setup.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                            }
                        })
                        .UseHttpsRedirection()
                        .UseRewriter(rewriteOptions)
                        .UseDefaultFiles()
                        .UseStaticFiles();
                },
                postConfigure: host =>
                {
                    return host
                    .UseAuthentication()
                    .UseAuthorization();
                });
        }
    }
}

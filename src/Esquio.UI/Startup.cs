using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.Services;
using Esquio.UI.Infrastructure.Security.ApiKey;
using Esquio.UI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                .AddSingleton<IDiscoverToggleTypesService, DiscoverToggleTypesService>()
                //.AddAuthorization()
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
                .AddScoped<IApiKeyStore,DefaultApiKeyStore>()
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            EsquioUIApiConfiguration.Configure(app,
                preConfigure: host =>
                {
                    var rewriteOptions = new RewriteOptions()
                       .AddRewrite(@"^(?!.*(api\/|.*\.(js|css|ico)|fonts\/|img\/|static\/|swagger*|ws\/*)).*$", "index.html", skipRemainingRules: true);

                    return host
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

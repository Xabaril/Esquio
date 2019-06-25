using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

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
                .AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                //.AddApiKey()
                .AddJwtBearer(options =>
                {
                    Configuration.Bind("Security:Jwt", options);
                    var authority = options.Authority;

                    options.Events = new JwtBearerEvents();
                    options.Events.OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                    };
                    options.Events.OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    };
                    options.Events.OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;
                    };

                    options.Events.OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    };
                });

            EsquioUIApiConfiguration.ConfigureServices(services)
                .AddDbContext<StoreDbContext>(options =>
                {
                    options.UseSqlServer(Configuration["Data:EsquioConnectionString"], setup =>
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
                       .AddRewrite(@"^(?!.*(api\/|static\/|swagger*|ws/*)).*$", "index.html", skipRemainingRules: true);

                    return host
                        .UseHttpsRedirection()
                        .UseRewriter(rewriteOptions)
                        .UseDefaultFiles()
                        .UseStaticFiles();
                },
                postConfigure: host =>
                {
                    return host
                    .UseAuthorization()
                    .UseAuthentication();
                });
        }
    }
}

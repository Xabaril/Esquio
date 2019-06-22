using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            //services.AddAuthentication()
            //    .AddApiKey();

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
            EsquioUIApiConfiguration.Configure(app, host =>
            {
                var rewriteOptions = new RewriteOptions()
                   .AddRewrite(@"^(?!.*(api\/|static\/|swagger*|ws/*)).*$", "index.html", skipRemainingRules: true);

                host
                    .UseHttpsRedirection()
                    .UseRewriter(rewriteOptions)
                    .UseDefaultFiles()
                    .UseStaticFiles();

                return host;
            });
        }
    }
}

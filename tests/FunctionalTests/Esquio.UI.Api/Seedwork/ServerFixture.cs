using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Services;
using Esquio.UI.Host.Infrastructure.Services;
using Esquio.UI.Store.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Seedwork
{
    public class ServerFixture
    {
        static Checkpoint _checkpoint = new Checkpoint()
        {
            TablesToIgnore = new string[] { "__EFMigrationsHistory" },
            WithReseed = true
        };

        public TestServer TestServer { get; private set; }

        public Given Given { get; private set; }


        private IHost _host;

        public ServerFixture()
        {
            InitializeTestServer();
        }

        private void InitializeTestServer()
        {
            _host = new HostBuilder()
                .ConfigureWebHost(builder =>
                {
                    builder
                    .ConfigureServices(services => services.AddSingleton<IServer>(serviceProvider => new TestServer(serviceProvider)))
                    .UseStartup<TestStartup>();
                })
                .ConfigureAppConfiguration((_, cfg) =>
                 {
                     cfg.AddJsonFile("appsettings.json", optional: false);
                 }).Build();

            _host.StartAsync().Wait();

            _host.MigrateDbContext<StoreDbContext>((store, sp) =>
                {

                });

            TestServer = _host.GetTestServer();
            Given = new Given(this);
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> func)
        {
            using (var scope = _host.Services.GetService<IServiceScopeFactory>()
                .CreateScope())
            {
                await func(scope.ServiceProvider);
            }
        }

        public async Task ExecuteDbContextAsync(Func<StoreDbContext, Task> func)
        {
            await ExecuteScopeAsync(sp => func(sp.GetService<StoreDbContext>()));
        }

        internal static void ResetDatabase()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", optional: true);

            var connectionString = configurationBuilder.Build()
                .GetConnectionString("Esquio");

            var task = _checkpoint.Reset(connectionString);
            task.Wait();
        }
    }

    [CollectionDefinition(nameof(AspNetCoreServer))]
    public class AspNetCoreServer
        : ICollectionFixture<ServerFixture>
    {

    }

    class TestStartup
    {
        private IConfiguration _configuration;

        public TestStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            EsquioUIApiConfiguration.ConfigureServices(services)
                .AddSingleton<IDiscoverToggleTypesService, DiscoverToggleTypesService>()
                .AddAuthorization()
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = "secured";
                    setup.DefaultChallengeScheme = "secured"; 
                })
                .AddApiKey()
                .AddTestServer()
                .AddPolicyScheme("secured", "Authorization TestServer or ApiKey", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var apiKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();

                        if (apiKey != null)
                        {
                            return ApiKeyAuthenticationDefaults.ApiKeyScheme;
                        }

                        return TestServerDefaults.AuthenticationScheme; ;
                    };
                })
                .Services
                .AddDbContext<StoreDbContext, StoreDbContext>(setup => setup.SetupDbStoreFromEnvironment(_configuration));
        }

        public void Configure(IApplicationBuilder app)
        {
            EsquioUIApiConfiguration.Configure(app, _ => _, host =>
            {
                return host.UseAuthentication().UseAuthorization();
            });
        }
    }
}

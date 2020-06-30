using System.Data.Common;
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
using Newtonsoft.Json;

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


        private static IHost _host;

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
                     cfg.AddJsonFile("appsettings.json", optional: false).AddEnvironmentVariables();
                     _checkpoint.DbAdapter = Convert.ToBoolean(cfg.Build()["Store:UseNpgSql"]) ? DbAdapter.Postgres : DbAdapter.SqlServer;
                     Console.WriteLine($"Using DBAdapter {_checkpoint.DbAdapter}");
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
            => await ExecuteScopeAsync(sp => func(sp.GetService<StoreDbContext>()));

        internal static void ResetDatabase()
        {
            // Get new scope
            using var scope = _host.Services.GetService<IServiceScopeFactory>().CreateScope();
            // Get fresh dbContext
            using var context = scope.ServiceProvider.GetService<StoreDbContext>();
            // Get Db Connection
            context.Database.OpenConnection();
            var connection = context.Database.GetDbConnection();
            // Reset Db            
            if (_checkpoint.DbAdapter == DbAdapter.Postgres){
                _checkpoint.WithReseed = false;
                _checkpoint.SchemasToInclude = new string[] {"public"};
            }
            var task = _checkpoint.Reset(connection);
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

                        return TestServerDefaults.AuthenticationScheme;
                    };
                })
                .Services
                .AddEntityFramework(_configuration);
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

using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Services;
using Esquio.UI.Host.Infrastructure.Services;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
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
        static Checkpoint _checkpoint = new Checkpoint();

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
                .ConfigureAppConfiguration((_, configurationBuilder) =>
                 {
                     configurationBuilder.AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();

                     var configuration = configurationBuilder.Build();
                     _checkpoint.DbAdapter = DbAdapterFactory.CreateFromProvider(configuration["Data:Store"]);
                 }).Build();

            _host.StartAsync().Wait();

            _host.MigrateDbContext<StoreDbContext>((_, __) => { });

            TestServer = _host.GetTestServer();
            Given = new Given(this);
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> func)
        {
            using (var scope = _host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                await func(scope.ServiceProvider);
            }
        }

        public async Task ExecuteDbContextAsync(Func<StoreDbContext, Task> func)
            => await ExecuteScopeAsync(sp => func(sp.GetService<StoreDbContext>()));

        internal static void ResetDatabase()
        {
            using var scope = _host.Services.GetService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<StoreDbContext>();

            context.Database.OpenConnection();
            using var connection = context.Database.GetDbConnection();

            _checkpoint.ConfigureForCurrentAdapter();
           
            _checkpoint.Reset(connection)
                .Wait();
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

        public IWebHostEnvironment Environment { get; }

        public TestStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            EsquioUIApiConfiguration.ConfigureServices(services,_configuration)
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
                .AddEntityFramework(_configuration, Environment);
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

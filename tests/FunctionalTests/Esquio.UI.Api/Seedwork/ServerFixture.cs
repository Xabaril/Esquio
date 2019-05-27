using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using System.IO;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Seedwork
{
    public class ServerFixture
    {
        static Checkpoint _checkpoint = new Checkpoint();

        public TestServer TestServer { get; private set; }

        public ServerFixture()
        {
            InitializeTestServer();
        }

        private void InitializeTestServer()
        {
            var testServer = new TestServer();

            var host = Host.CreateDefaultBuilder()
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .ConfigureAppConfiguration((_, cfg) =>
                 {
                     cfg.AddJsonFile("appsettings.json", optional: false);
                 })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder
                     .UseServer(testServer)
                     .UseStartup<TestStartup>();
                 }).Build();

            host.StartAsync().Wait();

            host.MigrateDbContext<StoreDbContext>((store, sp) =>
            {

            });

            TestServer = host.GetTestServer();
        }

        //private void MigrateDatabase()
        //{
        //    var context = TestServer.Host
        //        .MigrateDbContext<StoreDbContext>((store, sp) =>
        //        {

        //        });
        //}

        internal static void ResetDatabase()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", optional: true);

            var connectionString = configurationBuilder.Build()
                .GetConnectionString("Esquio");

            _checkpoint.Reset(connectionString).Wait();
        }
    }

    [CollectionDefinition(nameof(AspNetCoreServer))]
    public class AspNetCoreServer
        : IClassFixture<ServerFixture>
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
                .AddAuthorization()
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = TestServerDefaults.AuthenticationScheme;
                    setup.DefaultChallengeScheme = TestServerDefaults.AuthenticationScheme;
                })
                .AddTestServer()
                .Services
                .AddDbContext<StoreDbContext>(setup =>
                {
                    setup.UseSqlServer(_configuration.GetConnectionString("Esquio"), opt =>
                    {
                        opt.MigrationsAssembly(typeof(ServerFixture).Assembly.FullName);
                    });
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            EsquioUIApiConfiguration.Configure(app, host =>
            {
                return host
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization();
            });
        }
    }
}

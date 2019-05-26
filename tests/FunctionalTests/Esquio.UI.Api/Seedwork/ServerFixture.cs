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
using System.IO;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Seedwork
{
    public class ServerFixture
    {
        public TestServer TestServer { get; }

        public ServerFixture()
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

            TestServer = host.GetTestServer();
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
                .AddDbContext<StoreDbContext>(setup=>
                {
                    setup.UseSqlServer(_configuration.GetConnectionString("Esquio"), opt =>
                    {
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

using Esquio.AspNetCore.Endpoints.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace UnitTests.Seedwork
{
    public class ServerFixture
    {
        public TestServer TestServer { get; set; }

        public ServerFixture()
        {
            var host = new HostBuilder()
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
            services.AddMvc()
                .Services
                .AddEsquio(setup=>
                {
                    setup.RegisterTogglesFromAssembly(typeof(AllwaysOnToggle).Assembly);
                })
                .AddAspNetCoreDefaultServices()
                .AddConfigurationStore(_configuration, "Esquio");
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapEsquio("esquio");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }

    public class ScenariosController : Controller
    {
        [ActionName("MultipleEndpointsWithFeatureSample1")]
        [FeatureFilter(Name = "Sample1")]
        public IActionResult Sample1()
        {
            return Content("enabled");
        }

        [ActionName("MultipleEndpointsWithFeatureSample1")]
        public IActionResult Sample11()
        {
            return Content("Disabled");
        }

        [ActionName("MultipleEndpointsWithFeatureSample2")]
        [FeatureFilter(Name = "Sample2")]
        public IActionResult Sample2()
        {
            return Content("Enabled");
        }
        [ActionName("MultipleEndpointsWithFeatureSample2")]
        public IActionResult Sample22()
        {
            return Content("Disabled");
        }


        [FeatureFilter(Name = "Sample1")]
        public IActionResult SingleEndPointWithFeatureActive()
        {
            return Content("Enabled");
        }

        [FeatureFilter(Name = "Sample1")]
        [FeatureFilter(Name = "Sample1")]
        public IActionResult SingleEndPointWithMultipleFeatureActive()
        {
            return Content("Enabled");
        }

        [FeatureFilter(Name = "Sample1")]
        [FeatureFilter(Name = "Sample2")]
        public IActionResult SingleEndPointWithOneFeatureDisabled()
        {
            return Ok();
        }

        [FeatureFilter(Name = "Sample2")]
        public IActionResult SingleEndPointWithFeatureDisabled()
        {
            return Ok();
        }
    }
}

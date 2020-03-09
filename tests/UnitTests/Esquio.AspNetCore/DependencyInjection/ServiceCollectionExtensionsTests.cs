using Esquio.Abstractions.Providers;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Extensions
{
    public class addsquio_should
    {
        [Fact]
        public void include_mandatory_services_in_container()
        {
            var configurationBuilder = new ConfigurationBuilder();

            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddLogging()
                .AddEsquio()
                .AddAspNetCoreDefaultServices()
                .AddConfigurationStore(configurationBuilder.Build());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<IRoleNameProviderService>()
                .Should().NotBeNull();

            serviceProvider.GetService<IRoleNameProviderService>()
                .Should().NotBeNull();

            serviceProvider.GetServices<HeaderValueToggle>()
                .Should().NotBeNull();
        }
    }
}

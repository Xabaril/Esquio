using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddEsquio_include_mandatory_services_in_container()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddEsquio();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<IUserNameProviderService>()
                .Should().NotBeNull();

            serviceProvider.GetService<IRoleNameProviderService>()
                .Should().NotBeNull();

            serviceProvider.GetService<IGeoLocationProviderService>()
                .Should().NotBeNull();
        }
    }
}

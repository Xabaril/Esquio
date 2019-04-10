using Esquio.Abstractions;
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

            serviceProvider.GetService<IFeatureService>()
                .Should().NotBeNull();
        }
    }
}

using Esquio;
using Esquio.Abstractions;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.Toggles
{
    public class environment_toggle_should
    {
        private const string Development = "development";
        private const string Production = "production";
        private const string Environments = nameof(Environments);

        [Fact]
        public void throw_if_environment_provider_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new HostEnvironmentToggle(null);
            });
        }

        [Fact]
        public async Task be_not_active_if_current_environment_is_null()
        {
            var toggle = Build
                .Toggle<HostEnvironmentToggle>()
                .AddParameter(Environments, $"{Development}:{Production}")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var hostEnvironment = new FakeHostEnvironment(null);

            var active = await new HostEnvironmentToggle(hostEnvironment).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_parameter_value_is_null()
        {
            var toggle = Build
                .Toggle<HostEnvironmentToggle>()
                .AddParameter(Environments, Development)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var hostEnvironment = new FakeHostEnvironment(null);

            var active = await new HostEnvironmentToggle(hostEnvironment).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_current_environment_is_not_configured()
        {
            var toggle = Build
                .Toggle<HostEnvironmentToggle>()
                .AddParameter(Environments, Production)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var hostEnvironment = new FakeHostEnvironment(Development);

            var active = await new HostEnvironmentToggle(hostEnvironment).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured()
        {
            var toggle = Build
                .Toggle<HostEnvironmentToggle>()
                .AddParameter(Environments, Development)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var hostEnvironment = new FakeHostEnvironment(Development);

            var active = await new HostEnvironmentToggle(hostEnvironment).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured_with_different_case()
        {
            var toggle = Build
                .Toggle<HostEnvironmentToggle>()
                .AddParameter(Environments, Development)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var hostEnvironment = new FakeHostEnvironment("DeVeLoPmEnT");

            var active = await new HostEnvironmentToggle(hostEnvironment).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeTrue();
        }

        [Fact]
        public async Task be_active_if_current_environment_is_configured_in_a_list()
        {
            var toggle = Build
                .Toggle<HostEnvironmentToggle>()
                .AddParameter(Environments, $"{Development};{Production}")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var hostEnvironment = new FakeHostEnvironment("Development");

            var active = await new HostEnvironmentToggle(hostEnvironment).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeTrue();
        }
    }
}

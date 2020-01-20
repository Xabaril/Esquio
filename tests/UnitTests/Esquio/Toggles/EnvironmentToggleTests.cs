using Esquio;
using Esquio.Abstractions;
using Esquio.Toggles;
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
                new EnvironmentToggle(null);
            });
        }

        [Fact]
        public async Task be_not_active_if_current_environment_is_null()
        {
            var toggle = Build
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, $"{Development}:{Production}")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => null);

            var active = await new EnvironmentToggle(environmentNameProvider).IsActiveAsync(
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
                .Toggle<EnvironmentToggle>()
                .AddOneParameter("Environments","Production")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => Development);

            var active = await new EnvironmentToggle(environmentNameProvider).IsActiveAsync(
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
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, Production)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => Development);

            var active = await new EnvironmentToggle(environmentNameProvider).IsActiveAsync(
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
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, Development)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "development");

            var active = await new EnvironmentToggle(environmentNameProvider).IsActiveAsync(
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
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, Development)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "DeVeLoPmEnT");

            var active = await new EnvironmentToggle(environmentNameProvider).IsActiveAsync(
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
                .Toggle<EnvironmentToggle>()
                .AddOneParameter(Environments, $"{Development};{Production}")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();
            
            var environmentNameProvider = new DelegatedEnvironmentNameProviderService(() => "DeVeLoPmEnT");

            var active = await new EnvironmentToggle(environmentNameProvider).IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should().BeTrue();
        }
    }
}

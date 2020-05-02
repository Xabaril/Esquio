using Esquio;
using Esquio.Abstractions;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class hostname_toggle_tests
    {
        private const string HostNames = nameof(HostNames);

        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new HostNameToggle(null);
            });
        }

        [Fact]
        public async Task be_not_active_if_host_is_null()
        {
            var toggle = Build
                .Toggle<HostNameToggle>()
                .AddOneParameter(HostNames, "localhost")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new HostNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_hostname_is_not_allowed()
        {
            var toggle = Build
                .Toggle<HostNameToggle>()
                .AddOneParameter(HostNames, "sourcecodehost")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("localhost", 8080);
        
            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new HostNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_hostname_is_allowed()
        {
            var toggle = Build
                .Toggle<HostNameToggle>()
                .AddOneParameter(HostNames, "localhost")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("localhost", 8080);

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new HostNameToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }
    }
}

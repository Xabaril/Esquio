using Esquio;
using Esquio.Abstractions;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class serverip_toggle_tests
    {
        private const string IpAddresses = nameof(IpAddresses);

        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ServerIpAddressToggle(null);
            });
        }

        [Fact]
        public async Task be_not_active_if_localip_is_empty()
        {
            var toggle = Build
                .Toggle<ServerIpAddressToggle>()
                .AddParameter(IpAddresses, "127.0.0.1;127.0.0.2")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Connection
                .LocalIpAddress = null; 

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new ServerIpAddressToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_user_agent_is_not_allowed()
        {
            var toggle = Build
                .Toggle<ServerIpAddressToggle>()
                .AddParameter(IpAddresses, "127.0.0.1;127.0.0.2")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Connection
                .LocalIpAddress = IPAddress.Parse("127.0.0.4");

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new ServerIpAddressToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_user_agent_is_allowed()
        {
            var toggle = Build
               .Toggle<ServerIpAddressToggle>()
               .AddParameter(IpAddresses, "127.0.0.1;127.0.0.2")
               .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Connection
                .LocalIpAddress = IPAddress.Parse("127.0.0.1");

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new ServerIpAddressToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_RING_NAME,
                   toggle));

            active.Should().BeTrue();
        }
    }
}

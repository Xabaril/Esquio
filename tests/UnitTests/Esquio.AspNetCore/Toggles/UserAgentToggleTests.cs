using Esquio;
using Esquio.Abstractions;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class useragent_toggle_tests
    {
        private const string Browsers = nameof(Browsers);

        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new UserAgentToggle(null);
            });
        }

        [Fact]
        public async Task be_not_active_if_user_agent_is_empty()
        {
            var toggle = Build
                .Toggle<UserAgentToggle>()
                .AddParameter(Browsers, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36;Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.Request
                .Headers
                .Add("user-agent", "");

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new UserAgentToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_not_active_if_user_agent_is_not_allowed()
        {
            var toggle = Build
                .Toggle<UserAgentToggle>()
                .AddParameter(Browsers, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36;Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.Request
                .Headers
                .Add("user-agent", new StringValues("Internet Explorer 11"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new UserAgentToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeFalse();
        }

        [Fact]
        public async Task be_active_if_user_agent_is_allowed()
        {
            var toggle = Build
                .Toggle<UserAgentToggle>()
                .AddParameter(Browsers, "Firefox")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.Request
                .Headers
                .Add("user-agent", new StringValues("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0"));

            var contextAccessor = new FakeHttpContextAccessor(context);

            var active = await new UserAgentToggle(contextAccessor).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeTrue();
        }
    }
}

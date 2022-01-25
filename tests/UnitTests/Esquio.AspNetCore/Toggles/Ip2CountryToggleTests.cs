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
    public class ipcountry_toggle_should
    {
        private const string Countries = nameof(Countries);

        [Fact(Skip ="issues")]
        public void throw_if_httpcontextaccesor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Ip2CountryToggle(null, new FakeHttpClientFactory());
            });
        }

        [Fact(Skip = "issues")]
        public void throw_if_httpclientfactory_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Ip2CountryToggle(new FakeHttpContextAccessor(), null);
            });
        }

        [Fact(Skip = "issues")]
        public async Task be_active_if_configured_country_is_the_same_as_remote_ip_address()
        {
            var toggle = Build
                .Toggle<Ip2CountryToggle>()
                .AddParameter(Countries, "ES")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.Connection.RemoteIpAddress = IPAddress.Parse("2.142.250.6");

            var contextAccessor = new FakeHttpContextAccessor(context);
            var httpClientFactory = new FakeHttpClientFactory();

            var active = await new Ip2CountryToggle(contextAccessor, httpClientFactory).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeTrue();
        }
        [Fact(Skip = "issues")]
        public async Task be_active_if_configured_country_is_the_same_as_remote_ip_address_with_different_case()
        {
            var toggle = Build
                .Toggle<Ip2CountryToggle>()
                .AddParameter(Countries, "es")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.Connection.RemoteIpAddress = IPAddress.Parse("2.142.250.6");

            var contextAccessor = new FakeHttpContextAccessor(context);
            var httpClientFactory = new FakeHttpClientFactory();

            var active = await new Ip2CountryToggle(contextAccessor, httpClientFactory).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeTrue();
        }
        [Fact(Skip = "issues")]
        public async Task be_inactive_if_configured_country_is_not_the_same_as_remote_ip_address()
        {
            var toggle = Build
                .Toggle<Ip2CountryToggle>()
                .AddParameter(Countries, "FR")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.Connection.RemoteIpAddress = IPAddress.Parse("2.142.250.6");

            var contextAccessor = new FakeHttpContextAccessor(context);
            var httpClientFactory = new FakeHttpClientFactory();

            var active = await new Ip2CountryToggle(contextAccessor, httpClientFactory).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeFalse();
        }
        [Fact(Skip = "issues")]
        public async Task be_active_if_one_of_the_configured_countries_is_the_same_as_remote_ip_address()
        {
            var toggle = Build
                .Toggle<Ip2CountryToggle>()
                .AddParameter(Countries, "FR;US;ES")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            context.Connection.RemoteIpAddress = IPAddress.Parse("2.142.250.6");

            var contextAccessor = new FakeHttpContextAccessor(context);
            var httpClientFactory = new FakeHttpClientFactory();

            var active = await new Ip2CountryToggle(contextAccessor, httpClientFactory).IsActiveAsync(
               ToggleExecutionContext.FromToggle(
                   feature.Name,
                   EsquioConstants.DEFAULT_PRODUCT_NAME,
                   EsquioConstants.DEFAULT_DEPLOYMENT_NAME,
                   toggle));

            active.Should().BeTrue();
        }
    }
}

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
    public class headervalue_toggle_tests
    {
        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new HeaderValueToggle(null);
            });
        }

        [Fact]
        public async Task be_active_when_configured_header_contains_specified_value_successfully_configured()
        {
            var toggle = Build
                .Toggle<HeaderValueToggle>()
                .AddOneParameter("HeaderName", "Accept")
                .AddOneParameter("HeaderValues", "application/json")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Accept", "application/json");

            var headerValueToggle = new HeaderValueToggle(new FakeHttpContextAccessor(context));

            var active = await headerValueToggle.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_not_active_when_headervalue__are_not_successfully_configured()
        {
            var toggle = Build
                .Toggle<HeaderValueToggle>()
                .AddOneParameter("HeaderName", "Accept")
                .AddOneParameter("HeaderValues", "text/xml")
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Accept", "application/json");

            var headerValueToggle = new HeaderValueToggle(new FakeHttpContextAccessor(context));

            var active = await headerValueToggle.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeFalse();
        }
    }
}

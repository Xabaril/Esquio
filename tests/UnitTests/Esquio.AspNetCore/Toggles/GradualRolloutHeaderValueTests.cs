using Esquio;
using Esquio.Abstractions;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class gradualrolloutheadervalue_should
    {
        [Fact]
        public void throw_if_partitioner_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var accessor = new FakeHttpContextAccesor();
                var partitioner = new DefaultValuePartitioner();

                new GradualRolloutHeaderValueToggle(null, accessor);
            });
        }

        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var partitioner = new DefaultValuePartitioner();

                new GradualRolloutHeaderValueToggle(partitioner, null);
            });
        }

        [Fact]
        public async Task be_active_when_claim_value_is_on_valid_partition()
        {
            var toggle = Build
                .Toggle<GradualRolloutHeaderValueToggle>()
                .AddOneParameter("HeaderName", "header-name")
                .AddOneParameter("Percentage", 100)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Request.Headers.Add(new KeyValuePair<string, StringValues>("header-name", "header-value"));

            var partitioner = new DefaultValuePartitioner();

            var gradualRolloutHeaderValue = new GradualRolloutHeaderValueToggle(partitioner, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutHeaderValue.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeTrue();
        }

        [Theory]
        [InlineData(40)]
        [InlineData(10)]
        [InlineData(70)]
        [InlineData(72)]
        [InlineData(30)]
        [InlineData(80)]
        [InlineData(100)]
        [InlineData(11)]
        [InlineData(33)]
        public async Task use_partition_for_claim_value(int percentage)
        {
            var valid = false;
            var header_value = default(string);
            do
            {
                header_value = Guid.NewGuid().ToString();
                var partition = new DefaultValuePartitioner().ResolvePartition(Constants.FeatureName + header_value, partitions: 100);

                if (partition <= percentage)
                {
                    valid = true;
                }

            } while (!valid);

            var toggle = Build
               .Toggle<GradualRolloutHeaderValueToggle>()
               .AddOneParameter("HeaderName", "header-name")
               .AddOneParameter("Percentage", percentage)
               .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Request.Headers.Add(new KeyValuePair<string, StringValues>("header-name", header_value));

            var partitioner = new DefaultValuePartitioner();

            var gradualRolloutHeaderValue = new GradualRolloutHeaderValueToggle(partitioner, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutHeaderValue.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeTrue();
        }
        [Fact]
        public async Task be_non_active_when_claim_value_is_not_on_valid_partition()
        {
            var toggle = Build
                .Toggle<GradualRolloutHeaderValueToggle>()
                .AddOneParameter("HeaderName", "header-name")
                .AddOneParameter("Percentage", 0)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Request.Headers.Add(new KeyValuePair<string, StringValues>("header-name", "header-value"));

            var partitioner = new DefaultValuePartitioner();

            var gradualRolloutHeaderValue = new GradualRolloutHeaderValueToggle(partitioner, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutHeaderValue.IsActiveAsync(
                ToggleExecutionContext.FromToggle(
                    feature.Name,
                    EsquioConstants.DEFAULT_PRODUCT_NAME,
                    EsquioConstants.DEFAULT_RING_NAME,
                    toggle));

            active.Should()
                .BeFalse();
        }

        private class FakeHttpContextAccesor
            : IHttpContextAccessor
        {
            public HttpContext HttpContext { get; set; }

            public FakeHttpContextAccesor() { }

            public FakeHttpContextAccesor(HttpContext context)
            {
                HttpContext = context;
            }
        }
    }
}

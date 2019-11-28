using Esquio.Abstractions;
using Esquio.AspNetCore.Toggles;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Builders;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class gradualrolloutsession_should
    {
        [Fact]
        public void throw_if_partitioner_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var store = new DelegatedValueFeatureStore((_, __) => null);
                var accessor = new FakeHttpContextAccesor();

                new GradualRolloutSessionToggle(null, store, accessor);
            });
        }

        [Fact]
        public void throw_if_httpcontextaccessor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var store = new DelegatedValueFeatureStore((_, __) => null);
                var partitioner = new DefaultValuePartitioner();

                new GradualRolloutSessionToggle(partitioner, store, null);
            });
        }

        [Fact]
        public async Task be_active_when_session_is_on_valid_partition()
        {
            var toggle = Build
                .Toggle<GradualRolloutSessionToggle>()
                .AddOneParameter("Percentage", 100)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Session = new FakeSession();

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var partitioner = new DefaultValuePartitioner();

            var gradualRolloutSession = new GradualRolloutSessionToggle(partitioner, store, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutSession.IsActiveAsync(Constants.FeatureName);

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
        public async Task use_partition_for_session(int percentage)
        {
            var valid = false;
            var sessionId = string.Empty;

            do
            {
                sessionId = Guid.NewGuid().ToString();
                var partition = new DefaultValuePartitioner().ResolvePartition(Constants.FeatureName + sessionId, partitions: 100);

                if (partition <= percentage)
                {
                    valid = true;
                }

            } while (!valid);

            var toggle = Build
               .Toggle<GradualRolloutSessionToggle>()
               .AddOneParameter("Percentage", percentage)
               .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Session = new FakeSession(sessionId);

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var partitioner = new DefaultValuePartitioner();

            var gradualRolloutSession = new GradualRolloutSessionToggle(partitioner, store, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutSession.IsActiveAsync(Constants.FeatureName);

            active.Should()
                .BeTrue();
        }

        [Fact]
        public async Task be_non_active_when_claim_value_is_not_on_valid_partition()
        {
            var toggle = Build
                .Toggle<GradualRolloutSessionToggle>()
                .AddOneParameter("Percentage", 0)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();
            context.Session = new FakeSession();

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var partitioner = new DefaultValuePartitioner();

            var gradualRolloutSession = new GradualRolloutSessionToggle(partitioner, store, new FakeHttpContextAccesor(context));

            var active = await gradualRolloutSession.IsActiveAsync(Constants.FeatureName);

            active.Should()
                .BeFalse();
        }

        [Fact]
        public async Task throw_when_session_is_not_active()
        {
            var toggle = Build
                .Toggle<GradualRolloutSessionToggle>()
                .AddOneParameter("Percentage", 100)
                .Build();

            var feature = Build
                .Feature(Constants.FeatureName)
                .AddOne(toggle)
                .Build();

            var context = new DefaultHttpContext();

            var store = new DelegatedValueFeatureStore((_, __) => feature);
            var partitioner = new DefaultValuePartitioner();

            var gradualRolloutSession = new GradualRolloutSessionToggle(partitioner, store, new FakeHttpContextAccesor(context));

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await gradualRolloutSession.IsActiveAsync(Constants.FeatureName);
            });
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
        private class FakeSession
            : ISession
        {
            string _sessionId;

            public FakeSession(string sessionId)
            {
                _sessionId = sessionId;
            }

            public FakeSession()
            {
                _sessionId = Guid.NewGuid().ToString();
            }
            public string Id => _sessionId;

            public bool IsAvailable => true;

            public IEnumerable<string> Keys => Enumerable.Empty<string>();

            public void Clear()
            {
            }

            public Task CommitAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task LoadAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public void Remove(string key)
            {
            }

            public void Set(string key, byte[] value)
            {
            }

            public bool TryGetValue(string key, out byte[] value)
            {
                value = default;
                return false;
            }
        }
    }
}

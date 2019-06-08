using Esquio.AspNetCore.Toggles;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Toggles
{
    public class gradualrolloutclaimvalue_should
    {
        [Fact]
        public async Task throw_if_store_service_is_null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var accessor = new FakeHttpContextAccesor();
                new GradualRolloutClaimValueToggle(null, accessor);
            });
        }

        [Fact]
        public async Task throw_if_httpcontextaccessor_is_null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var store = new DelegatedValueFeatureStore((_, __) => null);
                new GradualRolloutClaimValueToggle(store, null);
            });
        }

        //TODO: add pending tests 

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

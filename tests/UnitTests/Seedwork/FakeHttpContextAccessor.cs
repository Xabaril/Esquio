using Microsoft.AspNetCore.Http;

namespace UnitTests.Seedwork
{
    internal class FakeHttpContextAccessor
        : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }

        public FakeHttpContextAccessor() { }

        public FakeHttpContextAccessor(HttpContext context)
        {
            HttpContext = context;
        }
    }
}

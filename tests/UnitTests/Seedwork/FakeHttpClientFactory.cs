using System.Net.Http;

namespace UnitTests.Seedwork
{
    class FakeHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}

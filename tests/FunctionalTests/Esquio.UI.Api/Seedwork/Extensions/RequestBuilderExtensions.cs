using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.TestHost
{
    public static class RequestBuilderExtensions
    {
        public static Task<HttpResponseMessage> PutAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Put);
        }
        public static Task<HttpResponseMessage> DeleteAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Delete);
        }
    }
}

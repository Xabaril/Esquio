using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.TestHost
{
    public static class RequestBuilderExtensions
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static Task<HttpResponseMessage> PutAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Put);
        }
        public static Task<HttpResponseMessage> DeleteAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Delete);
        }
        public static Task<HttpResponseMessage> PostAsJsonAsync<TEntity>(this RequestBuilder requestBuilder, TEntity entity)
        {
            return requestBuilder.And(message =>
            {
                var body = JsonSerializer.Serialize(entity, options: _serializerOptions);
                message.Content = new StringContent(body, Encoding.UTF8, "application/json");

            }).SendAsync(HttpMethods.Post);
        }
        public static Task<HttpResponseMessage> PutAsJsonAsync<TEntity>(this RequestBuilder requestBuilder, TEntity entity)
        {
            return requestBuilder.And(message =>
            {
                var body = JsonSerializer.Serialize(entity, options: _serializerOptions);
                message.Content = new StringContent(body, Encoding.UTF8, "application/json");

            }).SendAsync(HttpMethods.Put);
        }
    }
}

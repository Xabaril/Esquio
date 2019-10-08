using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    public static class HttpContentExtensions
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static async Task<TEntity> ReadAs<TEntity>(this HttpContent httpContent)
        {
            var body = await httpContent.ReadAsStringAsync();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            {
                return await JsonSerializer.DeserializeAsync<TEntity>(stream, options: _serializerOptions);
            }
        }
    }
}

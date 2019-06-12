using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    public static class HttpContentExtensions
    {
        public static async Task<TEntity> ReadAs<TEntity>(this HttpContent httpContent)
        {
            var body = await httpContent.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TEntity>(body);
        }
    }
}

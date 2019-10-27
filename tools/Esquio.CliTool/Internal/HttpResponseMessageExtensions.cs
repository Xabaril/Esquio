using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal static class HttpResponseMessageExtensions
    {
        public static async Task<string> GetErrorDetailAsync(this HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();

                var problemDetails =  JsonSerializer.Deserialize<BadRequestResponse>(content,new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (problemDetails != null && problemDetails.Errors.Any())
                {
                    var builder = new StringBuilder();

                    foreach (var item in problemDetails.Errors)
                    {
                        builder.Append($"{item.Key}: {item.Value.First()}");
                    }

                    return builder.ToString();
                }
                else
                {
                    return problemDetails.Detail;
                }
            }
            else
            {
                return "Verify Uri connection is listen Esquio and the Api-Key is valid";
            }
        }

        public static async Task<string> GetContentDetailAsync(this HttpResponseMessage responseMessage)
        {
            var document = await JsonDocument.ParseAsync(
                await responseMessage.Content.ReadAsStreamAsync(),
                new JsonDocumentOptions());

            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions() { Indented = true }))
            {
                document.WriteTo(writer);
                await writer.FlushAsync();

                stream.Seek(0, SeekOrigin.Begin);
                return await reader.ReadToEndAsync();
            }
        }

        private class BadRequestResponse
        {
            public int Status { get; set; }

            public string Detail { get; set; }

            public Dictionary<string,IEnumerable<string>> Errors { get; set; }
        }
    }
}

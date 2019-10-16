using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Esquio.UI.Api.Infrastructure.Serialization
{
    class NumberToStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var converter = options.GetConverter(typeof(JsonElement)) as JsonConverter<JsonElement>;

                if (converter != null)
                {
                    return converter.Read(ref reader, type, options).ToString();
                }
            }

            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}

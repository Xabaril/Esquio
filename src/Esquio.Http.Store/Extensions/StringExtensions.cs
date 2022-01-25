using Esquio.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class StringExtensions
    {

        public static JsonSerializerOptions _serializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReadCommentHandling = JsonCommentHandling.Disallow
        };

        public static Feature ToFeature(this string featureConfiguration)
        {
            var content = JsonSerializer
                .Deserialize<DistributedStoreResponse>(featureConfiguration, _serializationOptions);

            var feature = new Feature(content.FeatureName);

            if (content.Enabled)
            {
                feature.Enabled();
            }
            else
            {
                feature.Disabled();
            }

            foreach (var toggleConfiguration in content.Toggles)
            {
                var toggle = new Toggle(toggleConfiguration.Key);

                toggle.AddParameters(
                    toggleConfiguration.Value.Select(p => new Parameter(p.Key, p.Value)));

                feature.AddToggle(toggle);
            }

            return feature;
        }

        private class DistributedStoreResponse
        {
            public string FeatureName { get; set; }

            public bool Enabled { get; set; }

            public Dictionary<string, Dictionary<string, string>> Toggles { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

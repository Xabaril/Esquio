using System.Collections.Generic;

namespace Esquio.Abstractions
{
    public sealed class ToggleExecutionContext
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }

        public string RingName { get; set; }

        public Dictionary<string,object> Data { get; set; }
    }
}

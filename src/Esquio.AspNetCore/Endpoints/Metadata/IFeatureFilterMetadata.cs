using System;

namespace Esquio.AspNetCore.Endpoints.Metadata
{
    internal interface IFeatureFilterMetadata
    {
        public string Names { get; }

        public string ProductName { get; }
    }

    internal class FeatureFilterMetadata
        : IFeatureFilterMetadata
    {
        public FeatureFilterMetadata(string names, string productName = null)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
            ProductName = productName;
        }

        public string Names { get; private set; }

        public string ProductName { get; private set; }
    }

}

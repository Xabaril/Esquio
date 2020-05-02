using System;

namespace Esquio.UI.Api.Shared.Models.ApiKeys.Details
{
    public class DetailsApiKeyResponse
    {
        public string Name { get; set; }

        public string ActAs { get; set; }

        public DateTime ValidTo { get; set; }
    }
}

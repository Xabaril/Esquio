using System;

namespace Esquio.UI.Api.Features.ApiKeys.Details
{
    public class DetailsApiKeyResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ValidTo { get; set; }
    }
}

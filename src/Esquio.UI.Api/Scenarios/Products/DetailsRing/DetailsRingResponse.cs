using System.Collections.Generic;

namespace Esquio.UI.Api.Scenarios.Products.DetailsRing
{
    public class DetailsRingResponse
    {
        public string ProductName { get; set; }

        public IEnumerable<DetailsRingResponseDetail> Rings { get; set; }
    }

    public class DetailsRingResponseDetail
    {
        public string RingName { get; set; }

        public bool Default { get; set; }
    }
}

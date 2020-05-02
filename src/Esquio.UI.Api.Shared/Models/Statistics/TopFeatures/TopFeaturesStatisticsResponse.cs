using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Statistics.TopFeatures
{
    public class TopFeaturesStatisticsResponse
    {
        public IEnumerable<TopFeaturesDetailsStatisticsResponse> TopFeaturesDetails { get; set; }
    }
    public class TopFeaturesDetailsStatisticsResponse
    {
        public string FeatureName { get; set; }

        public int Requests { get; set; }
    }
}

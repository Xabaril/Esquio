using System;
using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Statistics.Plot
{
    public class PlotStatisticsResponse
    {
        public IEnumerable<PlotPointStatisticsResponse> Points { get; set; }
    }
    public class PlotPointStatisticsResponse
    {
        public DateTime Date { get; set; }

        public int Value { get; set; }
    }
}

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.ApplicationInsights.DataContracts;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Processor
{
    public class EsquioProcessor : ITelemetryProcessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ITelemetryProcessor Next { get; set; } // Link processors to each other in a chain.

        public EsquioProcessor(ITelemetryProcessor next, IHttpContextAccessor httpContextAccessor)
        {
            Next = next;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Process(ITelemetry item)
        {
            AddEsquioProperties(item);
            this.Next.Process(item);
        }
        private void AddEsquioProperties(ITelemetry item)
        {
            var esquiocontextItems = _httpContextAccessor.HttpContext?.Items.Where(item => item.Key.ToString().StartsWith(HttpContextItemObserver.EsquioItemKeyName));
            
            ISupportProperties propTelemetry = (ISupportProperties)item;

            if (esquiocontextItems is object)
            {
                foreach (var esquioContextItem in esquiocontextItems)
                {
                    if (!propTelemetry.Properties.ContainsKey(esquioContextItem.Key.ToString()))
                    {
                        propTelemetry.Properties.Add(esquioContextItem.Key.ToString(), esquioContextItem.Value.ToString());
                    }
                }
            }
        }
    }
}

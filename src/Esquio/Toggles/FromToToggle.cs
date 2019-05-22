using Esquio.Abstractions;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignTypeParameter(ParameterName = From, ParameterType = "System.DateTime", ParameterDescription = "The from date (yyyy-MM-dd HH:mm:ss) interval when this toggle is activated.")]
    [DesignTypeParameter(ParameterName = To, ParameterType = "System.DateTime", ParameterDescription = "The to date (yyyy-MM-dd HH:mm:ss) interval when this toggle is activated.")]
    public class FromToToggle
        : IToggle
    {
        internal const string FORMAT_DATE = "yyyy-MM-dd HH:mm:ss";
        const string From = nameof(From);
        const string To = nameof(To);
        private readonly IRuntimeFeatureStore _featureStore;

        public FromToToggle(IRuntimeFeatureStore featureStore)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }
        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            var fromDate = DateTime.ParseExact(data.From.ToString(), FORMAT_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var toDate = DateTime.ParseExact(data.To.ToString(), FORMAT_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var now = DateTime.UtcNow;

            if (now > fromDate && now < toDate)
            {
                return true;
            }

            return false;
        }
    }
}
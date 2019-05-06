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
        const string To = nameof(From);
        private readonly IFeatureStore _featureStore;

        public FromToToggle(IFeatureStore featureStore)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }
        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var fromValue = toggle.GetParameterValue(From).ToString();
            var toValue = toggle.GetParameterValue(To).ToString();
            var fromDate = DateTime.ParseExact(fromValue, FORMAT_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var toDate = DateTime.ParseExact(toValue, FORMAT_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var now = DateTime.UtcNow;

            if (now > fromDate && now < toDate)
            {
                return true;
            }

            return false;
        }
    }
}
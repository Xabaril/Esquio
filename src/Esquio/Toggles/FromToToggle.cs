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

        public async Task<bool> IsActiveAsync(string applicationName, string featureName)
        {
            var fromValue = (string)await _featureStore
                 .GetToggleParameterValueAsync<FromToToggle>(applicationName, featureName, From);

            var toValue = (string)await _featureStore
                .GetToggleParameterValueAsync<FromToToggle>(applicationName, featureName, To);

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
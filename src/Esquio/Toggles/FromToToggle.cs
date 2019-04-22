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

        public async Task<bool> IsActiveAsync(IFeatureContext context)
        {
            var featureStore = context.ServiceProvider.GetService<IFeatureStore>();

            var fromValue = (string)await featureStore
                 .GetToggleParameterValueAsync<FromToToggle>(context.ApplicationName, context.FeatureName, From);

            var toValue = (string)await featureStore
                .GetToggleParameterValueAsync<FromToToggle>(context.ApplicationName, context.FeatureName, To);

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
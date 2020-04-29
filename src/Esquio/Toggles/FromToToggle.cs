using Esquio.Abstractions;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on current UTC date time.
    /// </summary>
    [DesignType(Description = "Current UTC date falls within the interval.", FriendlyName = "Between dates")]
    [DesignTypeParameter(ParameterName = From, ParameterType = EsquioConstants.DATE_PARAMETER_TYPE, ParameterDescription = "The from date (yyyy-MM-dd HH:mm:ss) interval when this toggle is activated.")]
    [DesignTypeParameter(ParameterName = To, ParameterType = EsquioConstants.DATE_PARAMETER_TYPE, ParameterDescription = "The to date (yyyy-MM-dd HH:mm:ss) interval when this toggle is activated.")]
    public class FromToToggle
        : IToggle
    {
        internal const string DEFAULT_FORMAT_DATE = "yyyy-MM-dd HH:mm:ss";
        internal const string SINGLE_DIGIT_FORMAT_DATE = "yyyy-M-d H:m:s";

        internal const string From = nameof(From);
        internal const string To = nameof(To);

        /// <inheritdoc/>
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var parseExactFormats = new string[] { DEFAULT_FORMAT_DATE, SINGLE_DIGIT_FORMAT_DATE };

            var fromDate = DateTime.ParseExact(
                context.Data[From].ToString(), 
                parseExactFormats, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.AssumeUniversal);

            var toDate = DateTime.ParseExact(
                context.Data[To].ToString(), 
                parseExactFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal);

            var now = DateTime.UtcNow;

            if (now > fromDate && now < toDate)
            {
                return new ValueTask<bool>(true);
            }

            return new ValueTask<bool>(false);
        }
    }
}
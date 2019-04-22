using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = "System.Int32", ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    [DesignTypeParameter(ParameterName = HeaderName, ParameterType = "System.String", ParameterDescription = "The name of the header to get the value and create partitions buckets over this.")]
    public class RolloutHeaderValueToggle
        : IToggle
    {
        const string HeaderName = nameof(HeaderName);
        const string Percentage = nameof(Percentage);
        const int Partitions = 10;

        public async Task<bool> IsActiveAsync(IFeatureContext context)
        {
            var store = context.ServiceProvider.GetService<IFeatureStore>();
            var contextAccessor = context.ServiceProvider.GetService<IHttpContextAccessor>();

            var headerName = (string)await store
                .GetToggleParameterValueAsync<RolloutHeaderValueToggle>(context.ApplicationName, context.FeatureName, HeaderName);

            var percentage = (int)await store
                .GetToggleParameterValueAsync<RolloutHeaderValueToggle>(context.ApplicationName, context.FeatureName, Percentage);

            var values = contextAccessor.HttpContext.Request.Headers[headerName];

            var headerValue = values != StringValues.Empty ? values.First() : "NoHeaderValue";

            var assignedPartition = Partitioner.ResolveToLogicalPartition(headerValue, Partitions);

            return assignedPartition <= ((Partitions * percentage) / 100);
        }
    }
}

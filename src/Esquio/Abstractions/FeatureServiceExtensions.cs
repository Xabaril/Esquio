using System;
using System.Threading.Tasks;

namespace Esquio.Abstractions
{
    public static class FeatureServiceExtensions
    {
        public static async Task Do(this IFeatureService featureService, string featureName, Action whenTrue, Action whenFalse, string applicationName = null)
        {
            if (await featureService.IsEnabledAsync(featureName, applicationName))
            {
                whenTrue();
            }
            else
            {
                whenFalse();
            }
        }
        public static async Task<TResult> Do<TResult>(this IFeatureService featureService, string featureName, Func<TResult> whenTrue, Func<TResult> whenFalse, string applicationName = null)
        {
            if (await featureService.IsEnabledAsync(featureName, applicationName))
            {
                return whenTrue();
            }
            else
            {
                return whenFalse();
            }
        }
    }
}

using System;

namespace Esquio.Abstractions
{
    public interface IFeatureServiceErrorBehavior
    {
        bool GetActivationStateOrThrow(string featureName, string productName, Exception exception);
    }
}

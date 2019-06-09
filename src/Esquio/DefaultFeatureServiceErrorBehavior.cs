using Esquio.Abstractions;
using System;

namespace Esquio
{
    internal class DefaultFeatureServiceErrorBehavior
        : IFeatureServiceErrorBehavior
    {
        private readonly FeatureErrorBehavior _errorBehavior;

        public DefaultFeatureServiceErrorBehavior(FeatureErrorBehavior errorBehavior)
        {
            _errorBehavior = errorBehavior;
        }

        public bool GetActivationStateOrThrow(string featureName, string productName, Exception exception)
        {
            switch (_errorBehavior)
            {
                case FeatureErrorBehavior.Throw:
                    {
                        throw exception;
                    }
                case FeatureErrorBehavior.SetAsActive:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}

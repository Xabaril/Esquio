using System;
using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class MetricEntityBuilder
    {
        private string _kind;
        private string _deploymentName;
        private string _featureName;
        private string _productName;
        private DateTime _dateTime;

        public MetricEntityBuilder WithKind(string kind)
        {
            _kind = kind;
            return this;
        }
        public MetricEntityBuilder WithDeploymentName(string deploymentName)
        {
            _deploymentName = deploymentName;
            return this;
        }

        public MetricEntityBuilder WithFeatureName(string featureName)
        {
            _featureName = featureName;
            return this;
        }

        public MetricEntityBuilder WithProductName(string productName)
        {
            _productName = productName;
            return this;
        }

        public MetricEntityBuilder WithDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
            return this;
        }

        public MetricEntity Build()
        {
            return new MetricEntity
            {
                FeatureName = _featureName,
                ProductName = _productName,
                DeploymentName = _deploymentName,
                Kind = _kind,
                DateTime = _dateTime
            };
        }
    }
}

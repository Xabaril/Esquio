using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class HistoryBuilder
    {
        private string _newValues;
        private string _oldValues;
        private string _featureName;
        private string _productName;

        public HistoryBuilder WithNewValues(string newValues)
        {
            _newValues = newValues;
            return this;
        }
        public HistoryBuilder WithOldValues(string oldValues)
        {
            _oldValues = oldValues;
            return this;
        }

        public HistoryBuilder WithFeatureName(string featureName)
        {
            _featureName = featureName;
            return this;
        }

        public HistoryBuilder WithProductName(string productName)
        {
            _productName = productName;
            return this;
        }

        public HistoryEntity Build()
        {
            return new HistoryEntity
            {
                FeatureName = _featureName,
                ProductName = _productName,
                NewValues = _newValues,
                OldValues = _oldValues
            };
        }
    }
}

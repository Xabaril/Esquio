using Esquio.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class HistoryBuilder
    {
        private string _newValues;
        private string _oldValues;
        private int _featureId;

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

        public HistoryBuilder WithFeatureId(int featureId)
        {
            _featureId = featureId;
            return this;
        }

        public HistoryEntity Build()
        {
            return new HistoryEntity
            {
                FeatureId = _featureId,
                NewValues = _newValues,
                OldValues = _oldValues
            };
        }
    }
}

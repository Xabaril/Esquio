using Esquio.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class FeatureTagBuilder
    {
        private TagEntity _tag;
        private FeatureEntity _feature;

        public FeatureTagBuilder WithFeature(FeatureEntity feature)
        {
            _feature = feature;
            return this;
        }
        public FeatureTagBuilder WithTag(TagEntity tag)
        {
            _tag = tag;
            return this;
        }

        public FeatureTagEntity Build()
        {
            return new FeatureTagEntity
            {
                FeatureEntity = _feature,
                TagEntity = _tag
            };
        }
    }
}

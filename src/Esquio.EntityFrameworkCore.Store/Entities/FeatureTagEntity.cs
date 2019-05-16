namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class FeatureTagEntity
    {
        public int FeatureEntityId { get; set; }

        public FeatureEntity FeatureEntity { get; set; }

        public int TagEntityId { get; set; }

        public TagEntity TagEntity { get; set; }
    }
}

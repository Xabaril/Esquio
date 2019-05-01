using Esquio.EntityFrameworkCore.Store.Entities;

namespace Esquio
{
    public static class FeatureExtensions
    {
        public static FeatureEntity To(this Feature feature)
        {
            return new FeatureEntity
            {
                Name = feature.Name,
                Description = feature.Description,
                Enabled = feature.Enabled,
                CreatedOn = feature.CreatedOn
            };
        }
    }
}

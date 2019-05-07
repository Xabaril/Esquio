using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;

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
                Enabled = feature.IsEnabled,
                CreatedOn = feature.CreatedOn
            };
        }
    }
}

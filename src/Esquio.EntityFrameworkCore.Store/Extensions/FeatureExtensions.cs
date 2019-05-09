using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;
using System.Linq;

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
                IsPreview = feature.IsPreview,
                CreatedOn = feature.CreatedOn,
                Toggles = feature
                    .GetToggles()
                    .Select(t => new ToggleEntity
                    {
                        Type = t.Type,
                        Parameters = t
                            .GetParameters()
                            .Select(p => new ParameterEntity
                            {
                                Name = p.Name,
                                Value = p.Value.ToString()
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}

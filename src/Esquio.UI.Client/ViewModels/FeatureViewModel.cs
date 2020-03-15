using Esquio.UI.Client.Services;
using System.ComponentModel.DataAnnotations;

namespace Esquio.UI.Client.ViewModels
{
    public class FeatureViewModel
    {
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The feature name must be at least 5 characters long.")]
        public string Name { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The feature description must be at least 5 characters long.")]
        public string Description { get; set; }

        public AddFeatureRequest ToRequest()
        {
            return new AddFeatureRequest
            {
                Name = Name,
                Description = Description
            };
        }
    }
}

using Esquio.UI.Client.Services;
using System.ComponentModel.DataAnnotations;

namespace Esquio.UI.Client.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The product name must be at least 5 characters long.")]
        public string Name { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The product description must be at least 5 characters long.")]
        public string Description { get; set; }

        public string DefaultRingName { get; set; }

        public AddProductRequest ToRequest()
        {
            return new AddProductRequest
            {
                Name = Name,
                Description = Description,
                DefaultRingName = DefaultRingName
            };
        }
    }
}

using Esquio.UI.Client.Services;
using System.ComponentModel.DataAnnotations;

namespace Esquio.UI.Client.ViewModels
{
    public class RingViewModel
    {
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The ring name must be at least 5 characters long.")]
        public string Name { get; set; }

        public AddRingRequest ToRequest()
        {
            return new AddRingRequest
            {
                Name = Name,
            };
        }
    }
}

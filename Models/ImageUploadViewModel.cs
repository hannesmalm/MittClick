using System.ComponentModel.DataAnnotations;

namespace MittClick.Models
{
    public class ImageUploadViewModel
    {
        [Required(ErrorMessage = "Please select an image.")]
        [Display(Name = "Image File")]
        public IFormFile ImageFile { get; set; }
    }

}

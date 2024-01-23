using System.ComponentModel.DataAnnotations;

namespace MittClick.Models.ViewModels
{
    public class ImageUploadViewModel
    {
        [Required(ErrorMessage = "Please select an image.")]
        [Display(Name = "Image File")]
        public IFormFile ImageFile { get; set; }
    }

}

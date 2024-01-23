using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models.ViewModels
{
    public class AddProjectViewModel
    {

        [Required(ErrorMessage = "Var god fyll i titel")]

        public string Title { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Var god fyll i beskrivning")]

        public string Description { get; set; }

        public IFormFile? ProjectImage { get; set; }
    }
}
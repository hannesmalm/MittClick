using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class AddProjectViewModel
    {

        [Required(ErrorMessage = "Var god fyll i titel")]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Sammanfattning")]

        public string Summary { get; set; }

        [Required(ErrorMessage = "Var god fyll i beskrivning")]
        [Display(Name = "Längre beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Projektbild")]
        public string ProjectImg { get; set; }
    }
}

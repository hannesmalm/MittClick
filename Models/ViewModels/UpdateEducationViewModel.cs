using System.ComponentModel.DataAnnotations;

namespace MittClick.Models.ViewModels
{
    public class UpdateEducationViewModel
    {
        public List<Education> Educations { get; set; }
        public Profile Profile { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        public string School { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        public int From { get; set; }
        public int To { get; set; }
    }
}

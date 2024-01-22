using System.ComponentModel.DataAnnotations;

namespace MittClick.Models.ViewModels
{
    public class UpdateSkillViewModel
    {
        public List<Skill> Skills { get; set; }
        public Profile Profile { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        [StringLength(100, ErrorMessage = "Färdighetsnamnet är för långt.")]
        public string Name { get; set; }
    }
}

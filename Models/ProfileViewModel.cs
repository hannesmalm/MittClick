using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class ProfileViewModel
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProfileId { get; set; }

        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string Information { get; set; }
        [Required]
        public int? Number { get; set; }
        [Required]
        public string Email { get; set; }

        public string CV {  get; set; }

        public string ProfileImg { get; set; }
        [Required(ErrorMessage = "Please enter a skill.")]
        public string NewSkill { get; set; }
        public List<String> Skills { get; set; } = new List<String>();

    }
}

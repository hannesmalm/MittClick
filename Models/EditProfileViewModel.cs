using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class EditProfileViewModel
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "Fyll i ditt namn tack")]
        public string Name { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string Description { get; set; }

        public string ProfileImg { get; set; }

        public int Resume { get; set; }
    }
}

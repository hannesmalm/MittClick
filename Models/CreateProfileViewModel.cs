using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class CreateProfileViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "Snel fyll förstnamnet!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Snel fyll andernamnet!")]
        public string LastName { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string? Information { get; set; }

        public string? ProfileImg { get; set; }

        public string? Resume { get; set; }
        
        public virtual ICollection<Skill> Skills { get; set; }
    }
}

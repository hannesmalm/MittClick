using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class Profile
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProfileId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string? Information { get; set; }

        public string? ProfileImg { get; set; }

        public string? Resume { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public virtual User User { get; set; }
        
        public virtual ICollection<Skill> Skills { get; set; }
        public Profile()
        {
            Skills = new HashSet<Skill>();
        }
    }
}

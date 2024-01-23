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

        [ForeignKey("Image")]
        public byte[]? ProfileImage { get; set; }

		[ForeignKey("User")]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public virtual User User { get; set; }
        public virtual List<Skill> Skills { get; set; } = new List<Skill>();
        public virtual List<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();
        public virtual List<Education> Educations { get; set; } = new List<Education>();
        public virtual List<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
    }
}

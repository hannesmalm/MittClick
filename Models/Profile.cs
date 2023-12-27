using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public string Name { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string Information { get; set; }

        public string ProfileImg { get; set; }

        public int Resume { get; set; }

        public string? UserId { get; set; }
        public virtual User User { get; set; }
    }
}

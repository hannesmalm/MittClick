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

        public string? Name { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string? Information { get; set; }

        public string? ProfileImg { get; set; }

        public string? Resume { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}

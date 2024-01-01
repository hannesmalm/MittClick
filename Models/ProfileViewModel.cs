using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MittClick.Models
{
    public class ProfileViewModel
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Information { get; set; }

        public string ProfileImg { get; set; }

        public string Resume { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}

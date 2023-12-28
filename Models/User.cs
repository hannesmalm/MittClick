using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class User : IdentityUser
    {
        [ForeignKey("ProfileId")]
        public int? ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
    }
}

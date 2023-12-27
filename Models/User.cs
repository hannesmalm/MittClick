using Microsoft.AspNetCore.Identity;

namespace MittClick.Models
{
    public class User : IdentityUser
    {
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}

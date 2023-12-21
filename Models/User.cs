using Microsoft.AspNetCore.Identity;

namespace MittClick.Models
{
    public class User : IdentityUser
    {
        public List<Message> RecievedMessages { get; set; }
        public List<Message> SentMessages { get; set; }
        public List<Message> UnreadMessages { get; set; }

    }
}

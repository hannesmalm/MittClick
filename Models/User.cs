using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class User : IdentityUser
    {
        public virtual List<Message> ReceivedMessages { get; set; }

        public virtual List<Message> SentMessages { get; set; }
    }
}

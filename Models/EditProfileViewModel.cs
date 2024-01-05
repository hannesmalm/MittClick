using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class EditProfileViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get;set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string Information { get; set; }

        public string ProfileImg { get; set; }

        public string Resume { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }

    }


}

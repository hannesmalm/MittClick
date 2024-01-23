using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required]
        public bool PrivateProfile { get; set; }

        public string Information { get; set; }

        public IFormFile? ProfileImage { get; set; }

    }


}

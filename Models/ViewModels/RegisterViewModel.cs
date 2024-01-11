using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MittClick.Models.ViewModels
{

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Välj ett användarnamn.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Välj ett lösenord.")]
        [DataType(DataType.Password)]
        [Compare("RepeatPassword")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Bekräfta lösenordet")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}

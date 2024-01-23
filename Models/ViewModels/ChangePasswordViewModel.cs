using System.ComponentModel.DataAnnotations;

namespace MittClick.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Fyll i ditt nuvarande lösenord")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Välj ett lösenord.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Bekräfta lösenordet")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

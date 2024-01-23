using System.ComponentModel.DataAnnotations;

namespace MittClick.Models.ViewModels
{
    public class UpdateContactInfoViewModel
    {
        public List<ContactInfo> ContactInfos { get; set; }
        public Profile Profile { get; set; }
        [Required]
        public string Info { get; set; }
        [Required]
        public string Type { get; set; }

    }
}

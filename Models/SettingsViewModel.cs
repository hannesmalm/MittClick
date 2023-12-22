using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace MittClick.Models
{
    public class SettingsViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Presentation { get; set; }
        public string CV { get; set; }
        public bool Private { get; set; }
        public bool IsActive { get; set; } 

    }
}

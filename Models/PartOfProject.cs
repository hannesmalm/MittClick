using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class PartOfProject
    {
        public string UId { get; set; }
        public int PId { get; set; }
        
        [Required]
        [ForeignKey(nameof(UId))]
        public virtual User User { get; set; }

        [Required]
        [ForeignKey(nameof(PId))]
        public virtual Project Project { get; set; }


    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class PartOfProject
    {
        [Required]
        [ForeignKey(nameof(UId))]
        public string UId { get; set; }
       
        [Required]
        [ForeignKey(nameof(PId))]
        public int PId { get; set; }
        
        public virtual User? User { get; set; }
 
        public virtual Project? Project { get; set; }
    }
}

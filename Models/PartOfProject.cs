using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class PartOfProject
    {
        public int UId { get; set; }
        public int PId { get; set; }
        
        [Required]
        [ForeignKey(nameof(UId))]
        public User User { get; set; }

        [Required]
        [ForeignKey(nameof(PId))]
        public Project Project { get; set; }
    }
}

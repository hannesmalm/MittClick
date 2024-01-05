using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class PartOfProject
    {
        public string UId { get; set; }
        public int PId { get; set; }

        [ForeignKey(nameof(UId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(PId))]
        public virtual Project Project { get; set; }
    }
}

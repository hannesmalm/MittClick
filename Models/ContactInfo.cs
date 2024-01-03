using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class ContactInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Type { get; set; }
        public string Info { get; set; }

        public string ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual User? User { get; set; }
    }
}

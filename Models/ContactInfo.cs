using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class ContactInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Type { get; set; }
        public string Info { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public int ProfileId { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}

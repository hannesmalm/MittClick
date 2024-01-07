using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class Skill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public int ProfileId { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}

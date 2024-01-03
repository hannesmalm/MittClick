using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string Name { get; set; }
        // Foreign Key för att hålla reda på vilken Profile färdigheten tillhör
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}

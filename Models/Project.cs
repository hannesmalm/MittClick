using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MittClick.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Var god fyll i titel")]
        public string Title { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Var god fyll i beskrivning")]
        public string Description { get; set; }

        public string ProjectImg { get; set; }

        [Required(ErrorMessage = "Projektet måste ha en projektledare")]
        [ForeignKey(nameof(ProjectLeader))]
        public string ProjectLeader { get; set; }

        [Required]
        public virtual required User User { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

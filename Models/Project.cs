using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string ProjectImage { get; set; }

        [Required(ErrorMessage = "Projektet måste ha en projektledare")]
        [ForeignKey("UserId")]
        public virtual User ProjectLeader { get; set; }

        public virtual ICollection<PartOfProject> PartOfProjects { get; set; }
    }
}

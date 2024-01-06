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

        [ForeignKey("Image")]
        public byte[]? ProjectImage { get; set; }

        [Required(ErrorMessage = "Projektet måste ha en projektledare")]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<PartOfProject> PartOfProjects { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models.ViewModels
{
    public class CreateProfileViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public string LastName { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string? Information { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public virtual ICollection<Education>? Educations { get; set; }

        public virtual ICollection<WorkExperience>? WorkExperiences { get; set; }

        public virtual ICollection<Skill>? Skills { get; set; }

        public virtual ICollection<ContactInfo>? ContactInfos { get; set; }
    }
}

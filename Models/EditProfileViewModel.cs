﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class EditProfileViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get;set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string Information { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public virtual ICollection<Skill>? Skills { get; set; }
        public virtual ICollection<ContactInfo>? ContactInfos { get; set; }
        public virtual ICollection<Education>? Educations { get; set; }
        public virtual ICollection<WorkExperience>? WorkExperiences { get; set; }

    }


}

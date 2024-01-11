﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class Profile
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProfileId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool PrivateProfile { get; set; }

        public string? Information { get; set; }

        [ForeignKey("Image")]
        public byte[]? ProfileImage { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
		public virtual ICollection<WorkExperience> WorkExperiences { get; set; }

		[ForeignKey("User")]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();


        public virtual ICollection<ContactInfo> ContactInfos { get; set; }
        public Profile()
        {
            Skills = new HashSet<Skill>();
            ContactInfos = new HashSet<ContactInfo>();
            //nya
            Educations = new HashSet<Education>();
            WorkExperiences = new HashSet<WorkExperience>();
        }
        

    }
}

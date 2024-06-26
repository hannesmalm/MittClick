﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class User : IdentityUser
    {

        public virtual List<Message> ReceivedMessages { get; set; }

        public virtual Profile Profile { get; set; }
        
        public virtual ICollection<PartOfProject> PartOfProjects { get; set; }

    }
}

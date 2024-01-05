﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class User : IdentityUser
    {
        public virtual Profile Profile { get; set; }

        public virtual ICollection<PartOfProject> PartOfProjects { get; set; }
    }
}



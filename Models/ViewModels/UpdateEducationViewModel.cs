﻿namespace MittClick.Models.ViewModels
{
    public class UpdateEducationViewModel
    {
        public List<Education> Educations { get; set; }
        public Profile Profile { get; set; }

        public string School { get; set; }
        public string Type { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MittClick.Models.ViewModels

{
    public class UpdateWorkExperienceViewModel
    {
		[Required]
		public string Workplace { get; set; }

		[Required]
		public string Role { get; set; }
		[Required]
		[Range(1900, 2099)]
		public int From { get; set; }

		[Range(1900, 2099)]
		public int To { get; set; }

		[Required]
        public List<WorkExperience> WorkExperiences {  get; set; }
        public Profile Profile { get; set; }
    }
}

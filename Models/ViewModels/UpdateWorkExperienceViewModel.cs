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
		public int From { get; set; }

		public int? To { get; set; }

		[Required]
        public List<WorkExperience> WorkExperiences {  get; set; }
        public Profile Profile { get; set; }
    }
}

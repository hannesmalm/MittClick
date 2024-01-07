using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace MittClick.Models
{
	public class WorkExperience
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		
		public string Workplace { get; set; }
	
		public string Role {  get; set; }

		[RegularExpression(@"^(19|20)\d{2}$")]
		public int From { get; set; }

		[Compare("From")]
		public int? To { get; set; }

		[ForeignKey(nameof(ProfileId))]
		public int ProfileId { get; set; }
		public virtual Profile? Profile { get; set; }
	}
}

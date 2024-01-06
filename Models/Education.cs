using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
	public class Education
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
	
		public string School { get; set; }
		
		public string Type { get; set; }
	
		public int From { get; set; }
		public int? To { get; set; }
		[ForeignKey(nameof(ProfileId))]
		public int ProfileId { get; set; }
		public virtual Profile? Profile { get; set; }
	}
}

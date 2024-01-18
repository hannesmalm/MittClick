using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
	public class Education
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required(ErrorMessage = "Obligatoriskt fält")]
		public string School { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält")]
        public int From { get; set; }

		[RegularExpression(@"^(19|20)\d{2}$")]
		[Compare("From")]
		public int? To { get; set; }

		[ForeignKey(nameof(ProfileId))]
		public int ProfileId { get; set; }

		public virtual Profile? Profile { get; set; }
	}
}

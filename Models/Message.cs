using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string SenderName { get; set; }
        public string? Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public int RecieverId { get; set; }
        [ForeignKey(nameof(RecieverId))]
        public User Reciever { get; set; }
        
    }
}

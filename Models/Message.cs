using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public int RecieverId { get; set; }

        [Required]
        public string RecieverName { get; set; }

        [Required]
        public string Text { get; set; }

        

        [ForeignKey(nameof(RecieverId))]
        public User Reciever { get; set; }

        public bool IsRead { get; internal set; }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MittClick.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        public virtual User? Sender { get; set; }

        public string? SenderId { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ditt namn")]
        public string SenderName { get; set; }

        [Required]
        public virtual User Receiver { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage ="Du kan inte skicka ett tomt meddelande")]
        public string Text { get; set; }

        [Required]
        public bool IsRead { get; set; }


        public Message()
        {
            IsRead = false;
        }
    }
}

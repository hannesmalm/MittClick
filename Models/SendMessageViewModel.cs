﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MittClick.Models
{
    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "Du måste ange ett avsändarnamn")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Du måste ange ett mottagarenamn")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Du kan inte skicka ett tomt meddelande")]
        public string Text { get; set; }
    }
}
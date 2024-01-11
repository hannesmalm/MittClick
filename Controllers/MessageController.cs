using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;
using MittClick.Models.ViewModels;
using NuGet.Protocol.Plugins;

namespace MittClick.Controllers
{
    public class MessageController : Controller
    {
        private UserManager<User> userManager;
        private readonly MittClickDbContext dbContext;

        public MessageController(UserManager<User> userManager, MittClickDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Send(string? senderId, string receiverId)
        {
            Profile receiverProfile = dbContext.Profiles.FirstOrDefault(p => p.UserId == receiverId);
            
            if (receiverProfile != null)
            {
                string receiverFullName = receiverProfile.FirstName + " " + receiverProfile.LastName;
            
                if (senderId != null)
                {
                    Profile senderProfile = dbContext.Profiles.FirstOrDefault(p => p.UserId == senderId);
                    string senderFullName = senderProfile.FirstName + " " + senderProfile.LastName;

                    SendMessageViewModel sendMessageViewModel = new SendMessageViewModel
                    {
                        SenderName = senderFullName,
                        ReceiverName = receiverFullName,
                        ReceiverId = receiverId
                    };
                    return View(sendMessageViewModel);
                }
                else
                {
                    SendMessageViewModel sendMessageViewModel = new SendMessageViewModel
                    {
                        ReceiverName = receiverFullName,
                        ReceiverId = receiverId
                    };
                    return View(sendMessageViewModel);
                }
            } 
            else
            {
                Console.WriteLine("receiverProfile var null");
                return View(null);
            }
        }

        [HttpPost]
        public IActionResult Send(SendMessageViewModel sendMessageViewModel)
        {
            string? senderId = userManager.GetUserId(User);
            string senderName = sendMessageViewModel.SenderName;
            string receiverId = sendMessageViewModel.ReceiverId;
            string receiverName = sendMessageViewModel.ReceiverName;
            string text = sendMessageViewModel.Text;

            if (senderId != null)
            {
                Create(senderId, senderName, receiverId, receiverName, text);
            }
            else
            {
                Create(null, senderName, receiverId, receiverName, text);
            }

            return RedirectToAction("Index", "Home");
        }

        private Models.Message Create(string? senderId, string senderName, string receiverId, string receiverName, string text)
        {
            Models.Message message = new Models.Message
            {
                SenderId = senderId,
                SenderName = senderName,
                ReceiverId = receiverId,
                ReceiverName = receiverName,
                Text = text,
                IsRead = false
            };

            dbContext.Messages.Add(message);
            dbContext.SaveChanges();

            return (message);
        }

        [HttpGet]
        public IActionResult Inbox()
        {
            var currentUserId = userManager.GetUserId(User);
            var messages = dbContext.Messages
                           .Where(m => m.ReceiverId == currentUserId)
                           .OrderByDescending(m => m.IsRead)
                           .ThenByDescending(m => m.MessageId)
                           .ToList();

            return View(messages);
        }

        public IActionResult MarkAsRead(int messageId)
        {
            var message = dbContext.Messages.Find(messageId);

            if (message != null)
            {
                message.IsRead = true;
                dbContext.SaveChanges();
            }

            return RedirectToAction("Inbox");
        }

    }
}

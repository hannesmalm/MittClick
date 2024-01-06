using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;

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
        public IActionResult Send(string? senderId, string receiverUserName)
        {
            Profile receiverProfile = dbContext.Profiles.FirstOrDefault(p => p.UserName == receiverUserName);
            string receiverFullName = receiverProfile.FirstName + " " + receiverProfile.LastName;

            if (senderId != null)
            {
                Profile senderProfile = dbContext.Profiles.FirstOrDefault(p => p.UserId == senderId);
                string senderFullName = senderProfile.FirstName + " " + senderProfile.LastName;

                SendMessageViewModel sendMessageViewModel = new SendMessageViewModel
                {
                    SenderName = senderFullName,
                    ReceiverName = receiverFullName,
                };
                return View(sendMessageViewModel);
            }
            else
            {
                SendMessageViewModel sendMessageViewModel = new SendMessageViewModel
                {
                    ReceiverName = receiverFullName,
                };
                return View(sendMessageViewModel);
            }
        }

        [HttpPost]
        public IActionResult SendMessage(SendMessageViewModel messageViewModel)
        {
            // Här fortsätter din befintliga POST-kod för att spara meddelandet i databasen

            // ... (andra kod för att spara meddelandet i databasen)

            TempData["SuccessMessage"] = "Meddelandet skickades framgångsrikt!";
            return RedirectToAction("Index", "Home");
        }


        //[HttpGet]
        //[Authorize]
        //public IActionResult Inbox()
        //{
        //    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var messages = dbContext.Messages
        //        .Where(m => m.Receiver.Id == currentUserId)
        //        .ToList();

        //    return View(messages);
        //}

        //[HttpPost]
        //public IActionResult MarkAsRead(int messageId)
        //{
        //    // Markera meddelandet som läst och spara i databasen
        //    var message = dbContext.Messages.Find(messageId);
        //    if (message != null)
        //    {
        //        message.IsRead = true;
        //        dbContext.SaveChanges();
        //    }

        //    // Redirect tillbaka till inkorgen efter att ha markerat som läst
        //    return RedirectToAction("Inbox");
        //}
    }
}

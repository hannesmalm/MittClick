using Microsoft.AspNetCore.Mvc;
using MittClick.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
    public IActionResult SendMessage(string receiver, string sender)
    {
        if (User.Identity.IsAuthenticated)
        {
            SendMessageViewModel sendMessageViewModel = new SendMessageViewModel
            {
                SenderName = sender,
                ReceiverName = receiver,
            };

            return View(sendMessageViewModel);
        }
        else
        {
            SendMessageViewModel sendMessageViewModel = new SendMessageViewModel
            {
                ReceiverName = receiver,
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

    [HttpGet]
    [Authorize]

    public IActionResult Inbox()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var messages = dbContext.Messages
            .Where(m => m.ReceiverId == currentUserId)
            .ToList();

        return View(messages);
    }

    [HttpPost]
    public IActionResult MarkAsRead(int messageId)
    {
        // Markera meddelandet som läst och spara i databasen
        var message = dbContext.Messages.Find(messageId);
        if (message != null)
        {
            message.IsRead = true;
            dbContext.SaveChanges();
        }

        // Redirect tillbaka till inkorgen efter att ha markerat som läst
        return RedirectToAction("Inbox");
    }
}
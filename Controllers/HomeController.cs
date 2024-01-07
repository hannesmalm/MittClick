using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MittClick.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MittClickDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, MittClickDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        } 
        
        private async Task<byte[]> GetUserProfileImageAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return null;

            var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            return profile?.ProfileImage;
        }

        public async Task<IActionResult> IndexAsync(string id)
        {
            ViewBag.ProfileImage = await GetUserProfileImageAsync();
            var profileList = dbContext.Profiles.Include(p => p.User)
                                                    .Where(p => string.IsNullOrEmpty(id) || p.User.UserName == id)
                                                    .Select(p => new Profile
                                                    {
                                                        UserId = p.UserId,
                                                        // Map other properties as needed
                                                    })
                                                    .ToList();

            return View(profileList);
        }

       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

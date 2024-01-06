using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;
using System.Diagnostics;

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

        public IActionResult Index(string id)
        {
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

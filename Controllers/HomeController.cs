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

        //public IActionResult Index(string id)
        //{
        //    var userProfileList = dbContext.Profiles.Include(p => p.User)
        //                                            .Where(p => string.IsNullOrEmpty(id) || p.User.UserName == id)
        //                                            .Select(p => new UserProfileViewModel
        //                                            {
        //                                                UserId = p.UserId,
        //                                                UserName = p.User.UserName,
        //                                                HasProfile = true
        //                                            })
        //                                            .ToList();

        //    return View(userProfileList);
        //}

        public IActionResult Index(int projectId)
        {
            var projectList = dbContext.Projects.ToList();
            return View(projectList);
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

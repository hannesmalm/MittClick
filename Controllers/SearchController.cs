using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class SearchController : Controller
    {
        private MittClickDbContext profiles;
        public SearchController(MittClickDbContext service)
        {
            profiles = service;
        }
        public IActionResult Result()
        {
            var profileList = from profile in profiles.Profiles
                           select profile;
            return View(profileList.ToList());
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

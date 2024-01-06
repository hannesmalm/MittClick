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

        public IActionResult SearchEmpty()
        {
            var profileList = profiles.Profiles.ToList();
            return PartialView("SearchProfile", profileList);
        }

        public ActionResult SearchProfiles(string searchTerm)
        {
            var filteredProfiles = profiles.Profiles
                                           .Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm)) // Lägg till skills när de finns på main
                                           .ToList();

            return PartialView("SearchProfile", filteredProfiles);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

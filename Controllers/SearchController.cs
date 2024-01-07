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

        public ActionResult SearchRandom()
        {
            var allProfiles = profiles.Profiles;

            var validProfileIds = allProfiles
                .Select(p => p.ProfileId)
                .ToList();

            if (validProfileIds.Count == 0)
            {
                return PartialView("SearchProfile", new List<Profile>());
            }

            int seed = (int)DateTime.Now.Ticks; // Use current time in ticks as the seed

            Random randomId = new Random(seed);

            var randomProfiles = allProfiles
                .Where(p => validProfileIds.Contains(p.ProfileId))
                .ToList()  // Fetch the data from the database
                .OrderBy(p => randomId.Next()) // Use random order
                .Take(5)
                .ToList();

            return PartialView("SearchProfile", randomProfiles);
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}

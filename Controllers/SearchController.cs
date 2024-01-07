using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class SearchController : Controller
    {
        private IQueryable<Profile> profileQuery;
        private MittClickDbContext profiles;
        private UserManager<User> userManager;
        public SearchController(MittClickDbContext service, UserManager<User> userManager)
        {
            this.profiles = service;
            this.userManager = userManager;
        }

        public IActionResult Result()
        {
            var profileList = from profile in profiles.Profiles
                           select profile;
            return View(profileList.ToList());
        }

        public IActionResult SearchEmpty()
        {
            // Check om användare är inloggad
            if (!User.Identity.IsAuthenticated)
            {
                profileQuery = profileQuery.Where(p => !p.PrivateProfile);
            }

            var profileList = profileQuery.ToList();
            return PartialView("SearchProfile", profileList);
        }

        public ActionResult SearchProfiles(string searchTerm)
        {
            // Check om användare är inloggad
            if (!User.Identity.IsAuthenticated)
            {
                profileQuery = profileQuery.Where(p => !p.PrivateProfile);
            }

            // Fortsätt med vanlig söklogik
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                profileQuery = profileQuery.Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm));
            }

            var filteredProfiles = profileQuery.ToList();
            return PartialView("SearchProfile", filteredProfiles);
        }

        public ActionResult SearchRandom()
        {
            // Filtrera profiler baserat på om användaren är inloggad eller inte
            if (!User.Identity.IsAuthenticated)
            {
                profileQuery = profileQuery.Where(p => !p.PrivateProfile);
            }

            var validProfileIds = profileQuery.Select(p => p.ProfileId).ToList();

            if (validProfileIds.Count == 0)
            {
                return PartialView("SearchProfile", new List<Profile>());
            }

            int seed = (int)DateTime.Now.Ticks; //Uppdateras varje millisekund

            Random randomId = new Random(seed);

            var randomProfiles = allProfiles
                .Where(p => validProfileIds.Contains(p.ProfileId))
                .ToList()
                .OrderBy(p => randomId.Next())
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

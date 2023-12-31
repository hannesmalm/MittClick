using Microsoft.AspNetCore.Mvc;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class ProfileController : Controller
    {
        private MittClickDbContext dbContext;
        public ProfileController(MittClickDbContext dbContext)
        {
            this.dbContext = dbContext;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = TempData["UserId"] as string;
            ProfileViewModel model = new ProfileViewModel();
            model.UserId = userId;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProfileViewModel profile)
        {
            if (ModelState.IsValid)
            {
                dbContext.Profiles.Add(profile);
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); 
            }

            return View(profile);
        }

        public IActionResult Profile()
        {
            ProfileViewModel profileViewModel = new ProfileViewModel();
            return View(profileViewModel);
        }
        //public IActionResult CreateProfile()
        //{
        //    ProfileViewModel profileViewModel = new ProfileViewModel();
        //    return View(profileViewModel);
        //}

    }
}

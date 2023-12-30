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
        public ActionResult Create()
        {
            var userId = TempData["UserId"] as string;
            Console.WriteLine(userId);
            var model = new ProfileViewModel { UserId = userId };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProfileViewModel profile)
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

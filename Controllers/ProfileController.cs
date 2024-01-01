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
        public IActionResult Create(string userId)
        {
            
            var model = new ProfileViewModel
            {
                UserId = userId
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProfileViewModel model)
        {
            model.UserId = TempData["UI"] as string;
            Console.WriteLine(model.Name);
            Console.WriteLine(model.UserId);
            Console.WriteLine(model.Information);
            Console.WriteLine(model.Resume);

            if (ModelState.IsValid)
            {
                dbContext.Profiles.Add(model);
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); 
            }

            return View(model);
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

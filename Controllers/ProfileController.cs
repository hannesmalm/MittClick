using Microsoft.AspNetCore.Mvc;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class ProfileController : Controller
    {
        MittClickDbContext profiles;

        public ProfileController(MittClickDbContext service)
        {
            this.profiles = service;
        }
        public IActionResult Add()
        {
            ProfileViewModel profileViewModel = new ProfileViewModel();
            return View(profileViewModel);
        }

        public IActionResult Add(ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                profiles.Add(profileViewModel);
                profiles.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(profileViewModel);
            }
        }
    }
}

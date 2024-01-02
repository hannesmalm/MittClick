using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> userManager;
        private readonly MittClickDbContext dbContext;
        
        public ProfileController(MittClickDbContext dbContext, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> MyProfile()
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                string currentUserId = currentUser.Id.ToString();
                var currentUserProfile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == currentUserId);

                if (currentUserProfile != null) // om användaren har en profil
                {
                    return View(currentUserProfile);
                }

                else // om användaren inte har en profil, skapa den
                {
                    return RedirectToAction("CreateProfile", "Profile");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile(string userId)
        {
            var userProfile = dbContext.Profiles.Include(p => p.User)
                                                .Where(p => p.UserId == userId)
                                                .FirstOrDefault();

            if (userProfile != null)
            {
                return View(userProfile);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult CreateProfile()
        {
            CreateProfileViewModel createProfileViewModel = new CreateProfileViewModel();
            return View(createProfileViewModel);
        }

        [HttpPost]
        public IActionResult CreateProfile(CreateProfileViewModel createProfileViewModel)
        {
            //Testgrejer -------------------------------------------
            Console.WriteLine("Början på metod");
            var currentUser = userManager.GetUserAsync(User).Result;
            Console.WriteLine($"User ID: {currentUser.Id}");
            //Testgrejer ------------------------------------------

            if (ModelState.IsValid)
            {
                //Testgrejer ------------------------------------------
                Console.WriteLine("Modelstate är Valid");
                //Testgrejer --------------------------------------

                Profile newProfile = new Profile
                {
                    User = currentUser,
                    UserId = currentUser.Id,
                    FirstName = createProfileViewModel.FirstName,
                    LastName = createProfileViewModel.LastName,
                    PrivateProfile = createProfileViewModel.PrivateProfile,
                    Information = createProfileViewModel.Information,
                    ProfileImg = createProfileViewModel.ProfileImg,
                    Resume = createProfileViewModel.Resume,
                };

                //Testgrejer ---------------------------------------
                Console.WriteLine($"User ID: {currentUser.Id}");
                Console.WriteLine($"FirstName: {createProfileViewModel.FirstName}");
                Console.WriteLine($"LastName: {createProfileViewModel.LastName}");
                //Testgrejer ---------------------------------------


                dbContext.Profiles.Add(newProfile);
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Testgrejer ---------------------------------------
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (ModelError error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }

                Console.WriteLine("Modelstate är inte Valid");
                //Testgrejer ---------------------------------------

                return View(createProfileViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var userProfile = dbContext.Profiles.Include(p => p.User).FirstOrDefault(/* dina kriterier */);
            var user = userProfile.User; // Hämta användaren för profilen

            EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
            {
                ProfileId = userProfile.ProfileId,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PrivateProfile = userProfile.PrivateProfile,
                Information = userProfile.Information,
                ProfileImg = userProfile.ProfileImg,
                Resume = userProfile.Resume
            };

            return View(editProfileViewModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

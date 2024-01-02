using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private readonly MittClickDbContext dbContext;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, MittClickDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    loginViewModel.UserName,
                    loginViewModel.Password,
                    isPersistent: loginViewModel.RememberMe,
                    lockoutOnFailure: false);

                var currentUser = userManager.GetUserAsync(User).Result;
                string currentUserId = currentUser.Id.ToString();
                var currentUserProfile = dbContext.Profiles.FirstOrDefault(p => p.UserId == currentUserId);

                if (currentUserProfile != null)
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("MyProfile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Fel användarnamn/lösenord");
                    }
                }
                else
                {
                    return RedirectToAction("CreateProfile");
                }
            }
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    UserName = registerViewModel.UserName
                };

                var result = await userManager.CreateAsync(newUser, registerViewModel.Password);

                // när ny användare lagts till
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(newUser, isPersistent: true);

                    // går direkt till skapandet av ny profil
                    return RedirectToAction("CreateProfile");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerViewModel);
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
                    return RedirectToAction("CreateProfile");
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
            //Testgrejer -------------------------------------------

            if (ModelState.IsValid)
            {
                //Testgrejer ------------------------------------------
                Console.WriteLine("Modelstate är Valid");
                //Testgrejer ------------------------------------------

                Profile newProfile = new Profile
                {
                    User = currentUser,
                    UserName = currentUser.UserName,
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
                Console.WriteLine($"User ID: {currentUser.UserName}");
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

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var currentUser = await userManager.GetUserAsync(User);

            // Antag att du har en relation mellan ApplicationUser och Profile i databasen
            var userProfile = dbContext.Profiles
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == currentUser.Id);

            EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PrivateProfile = userProfile.PrivateProfile,
                Information = userProfile.Information,
                ProfileImg = userProfile.ProfileImg,
                Resume = userProfile.Resume
            };

            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User);

                var userProfile = dbContext.Profiles
                    .Include(p => p.User)
                    .FirstOrDefault(p => p.UserId == currentUser.Id);

                if (userProfile != null)
                {
                    userProfile.FirstName = editProfileViewModel.FirstName;
                    userProfile.LastName = editProfileViewModel.LastName;
                    userProfile.PrivateProfile = editProfileViewModel.PrivateProfile;
                    userProfile.Information = editProfileViewModel.Information;
                    userProfile.ProfileImg = editProfileViewModel.ProfileImg;
                    userProfile.Resume = editProfileViewModel.Resume;

                    dbContext.SaveChanges();
                }
                else
                {
                    return NotFound();
                }

                return RedirectToAction("MyProfile", "Account");
            }
            else
            {
                return View(editProfileViewModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }

}




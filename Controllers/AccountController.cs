using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                    return RedirectToAction("CreateProfile", new CreateProfileViewModel { UserId = currentUser.Id });
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

        public IActionResult GetProfile()
        {
            return View();
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
            if (ModelState.IsValid)
            {
                var currentUser = userManager.GetUserAsync(User).Result;

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

                dbContext.Profiles.Add(newProfile);
                dbContext.SaveChanges();

                return RedirectToPage("MyProfile", new { userId = currentUser.Id });
            }
            else
            {
                return View(createProfileViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile()
        {
            var currentUser = await userManager.GetUserAsync(User);

            EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
            {
                ProfileId = currentUser.Profile.ProfileId,
                FirstName = currentUser.Profile.FirstName,
                LastName = currentUser.Profile.LastName,
                PrivateProfile = currentUser.Profile.PrivateProfile,
                Information = currentUser.Profile.Information,
                ProfileImg = currentUser.Profile.ProfileImg,
                Resume = currentUser.Profile.Resume
            };

            return View(editProfileViewModel);
        }


        //[HttpPost]
        //public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var currentUser = await userManager.GetUserAsync(User);

        //        currentUser.Id = editProfileViewModel.UserId;

        //        if (currentUser.Profile == null)
        //        {
        //            currentUser.Profile = new Profile();
        //        }

        //        currentUser.Profile.ProfileId = editProfileViewModel.ProfileId;
        //        currentUser.Profile.PrivateProfile = editProfileViewModel.PrivateProfile;
        //        currentUser.Profile.Information = editProfileViewModel.Information;
        //        currentUser.Profile.ProfileImg = editProfileViewModel.ProfileImg;
        //        currentUser.Profile.Resume = editProfileViewModel.Resume;


        //        var result = await userManager.UpdateAsync(currentUser);

        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Profile");
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //        }
        //    }

        //    return View(editProfileViewModel);
        //}

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}


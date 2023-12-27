using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                    
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Fel användarnamn/lösenord");
                }
            }
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        public IActionResult AccountSettings()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();
                newUser.UserName = registerViewModel.UserName;
                var result =
                await userManager.CreateAsync(newUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(newUser, isPersistent: true);
                    return RedirectToAction("Index", "Home");
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

        public IActionResult Profile()
        {
            Profile profile = new Profile();
            return View(profile);
        }

        public async Task<IActionResult> EditProfile()
        {
            var currentUser = await userManager.GetUserAsync(User);

            EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
            {
                ProfileId = currentUser.Profile.ProfileId,
                Name = currentUser.Profile.Name,
                PrivateProfile = currentUser.Profile.PrivateProfile,
                Information = currentUser.Profile.Information,
                ProfileImg = currentUser.Profile.ProfileImg,
                Resume = currentUser.Profile.Resume
            };

            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User);

                currentUser.UserName = editProfileViewModel.Name;

                if (currentUser.Profile == null)
                {
                    currentUser.Profile = new Profile();
                }

                currentUser.Profile.ProfileId = editProfileViewModel.ProfileId;
                currentUser.Profile.PrivateProfile = editProfileViewModel.PrivateProfile;
                currentUser.Profile.Information = editProfileViewModel.Information;
                currentUser.Profile.ProfileImg = editProfileViewModel.ProfileImg;
                currentUser.Profile.Resume = editProfileViewModel.Resume;

                
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

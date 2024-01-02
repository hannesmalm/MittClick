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
                        return RedirectToAction("MyProfile", "Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Fel användarnamn/lösenord");
                    }
                }
                else
                {
                    return RedirectToAction("CreateProfile", "Profile");
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
                    return RedirectToAction("CreateProfile", "Profile");
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




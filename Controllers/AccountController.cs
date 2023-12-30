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
            ProfileViewModel profileViewModel = new ProfileViewModel();
            return View(profileViewModel);
        }

        public IActionResult EditProfile()
        {
            EditProfileViewModel editProfileViewModel = new EditProfileViewModel();
            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        // GET: /Account/ChangeUsername
        public IActionResult ChangeUsername()
        {
            return View();
        }

        // POST: /Account/ChangeUsername
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUsername(string newUsername)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                user.UserName = newUsername;
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Något gick fel");
                }
            }

            return View();
        }

        // GET: /Account/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (result.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Fel lösenord");
                }
            }

            return View();
        }


        [HttpGet]
        public IActionResult CreateProfile()
        {
            ProfileViewModel profileViewModel = new ProfileViewModel();
            return View(profileViewModel);

        }

        [HttpPost]
        public IActionResult CreateProfile(ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                // Om modellen är giltig kan du göra något med den här, t.ex. spara den i en databas.
                // Exempel:
                // _repository.SaveProfile(profileViewModel);

                return RedirectToAction("Index","Home"); // Redirect till en annan vy eller åtgärd
            }

            // Om modellen inte är giltig, återvänd till vyn med felmeddelanden
            return View(profileViewModel);
        }

        [HttpPost]
        public IActionResult AddSkill(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.NewSkill))
                {
                    var existingSkill = model.Skills
                        .Find(skill => skill.Equals(model.NewSkill, StringComparison.OrdinalIgnoreCase));

                    if (existingSkill == null)
                    {
                        model.Skills.Add(model.NewSkill);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.NewSkill), "Skill already exists.");
                    }
                }
            }

            // Återgå till sidan med den uppdaterade listan
            return RedirectToAction( "CreateProfile",model);
        }
        [HttpPost]
        public IActionResult RemoveSkill(ProfileViewModel model, string removeSkill)
        {
            var skillToRemove = model.Skills
                .Find(s => s.Equals(removeSkill, StringComparison.OrdinalIgnoreCase));

            if (skillToRemove != null)
            {
                model.Skills.Remove(skillToRemove);
            }

            // Återgå till sidan med den uppdaterade listan
            return View("CreateProfile", model);
        }
    }
}


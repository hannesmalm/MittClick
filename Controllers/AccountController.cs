using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult LogIn()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class ProjectController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private readonly MittClickDbContext dbContext;

        public ProjectController(UserManager<User> userManager, SignInManager<User> signInManager, MittClickDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProject()
        {
            AddProjectViewModel apvm = new AddProjectViewModel();
            return View(apvm);
        }

        [HttpPost]
        public IActionResult AddProject(AddProjectViewModel addProjectViewModel)
        {
            var currentUser = userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    // Konvertera från AddProjectViewModel till Project
                    var newProject = new Project
                    {
                        Title = addProjectViewModel.Title,
                        Summary = addProjectViewModel.Summary,
                        Description = addProjectViewModel.Description,
                        ProjectImg = addProjectViewModel.ProjectImg,
                        ProjectLeader = currentUser.Id,
                        User = currentUser
                        // Fortsätt att fylla i egenskaper här baserat på din modell
                    };

                    // Spara till din databas, antingen via en DbContext eller annan datahanteringsmetod
                    dbContext.Add(newProject);
                    dbContext.SaveChanges();
                    return RedirectToAction("Profile", "Account");

                }
                else
                {
                    // Efter att datan har sparats kan du omdirigera användaren till en annan vy eller handling
                    return RedirectToAction("AddProject");

                }
            }

            // Om ModelState inte är giltig, hantera fel eller visa vyn igen med felmeddelanden
            return View(addProjectViewModel);
        }
    }
}

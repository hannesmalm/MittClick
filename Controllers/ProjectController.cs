using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IActionResult Add()
        {
            AddProjectViewModel apvm = new AddProjectViewModel();
            return View(apvm);
        }

        public IActionResult Project(int projectId)
        {
            var project = dbContext.Projects
                .Include(p => p.PartOfProjects)
                    .ThenInclude(pp => pp.User)
                        .ThenInclude(u => u.Profile)
                .FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
            {
                return RedirectToAction("Index");
            }

            var currentUser = userManager.GetUserAsync(User).Result;
            var isCurrentUserPartOfProject = project.PartOfProjects.Any(pp => pp.UId == currentUser?.Id);

            ViewData["IsCurrentUserPartOfProject"] = isCurrentUserPartOfProject;

            var projectLeaderFullName = project.PartOfProjects
                .FirstOrDefault(pp => pp.UId == project.ProjectLeader)?
                .User.Profile.FirstName + " " + project.PartOfProjects
                    .FirstOrDefault(pp => pp.UId == project.ProjectLeader)?
                    .User.Profile.LastName;

            ViewData["ProjectLeaderFullName"] = projectLeaderFullName;

            return View("Project", project);
        }


        [HttpPost]
        public IActionResult Add(AddProjectViewModel addProjectViewModel)
        {
            var currentUser = userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    // Konvertera från AddProjectViewModel till Project
                    var newProject = new Models.Project
                    {
                        Title = addProjectViewModel.Title,
                        Summary = addProjectViewModel.Summary,
                        Description = addProjectViewModel.Description,
                        ProjectImg = addProjectViewModel.ProjectImg,
                        ProjectLeader = currentUser.Id,
                        User = currentUser
                    };

                    // Spara till din databas, antingen via en DbContext eller annan datahanteringsmetod
                    dbContext.Add(newProject);
                    dbContext.SaveChanges();

                    var partOfProject = new PartOfProject
                    {
                        UId = currentUser.Id,
                        PId = newProject.ProjectId,
                    };

                    dbContext.PartOfProjects.Add(partOfProject);
                    dbContext.SaveChanges();

                    return RedirectToAction("Profile", "Profile", new { userId = currentUser.Id });

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

        [HttpGet]
        public IActionResult AddUserToProject()
        {
            PartOfProject partOfProject = new PartOfProject();
            return View(partOfProject);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToProject(int projectId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            Console.WriteLine(currentUser);

            string userId = currentUser.Id.ToString();

            var existingParticipation = dbContext.PartOfProjects
                .FirstOrDefault(pp => pp.PId == projectId && pp.UId == userId);

            Console.WriteLine("user id: " + userId);
            Console.WriteLine("existingParticipation: " + existingParticipation);

            if (existingParticipation != null)
            {
                // Användaren är redan kopplad till projektet, hantera scenariot här
                TempData["Message"] = "Du är redan med i detta projekt";
                return RedirectToAction("Project", new { projectId });
            }

            var partOfProject = new PartOfProject
            {
                UId = userId,
                PId = projectId
            };

            dbContext.PartOfProjects.Add(partOfProject);
            dbContext.SaveChanges();

            return RedirectToAction("Project", new { projectId });
        }
    }
}

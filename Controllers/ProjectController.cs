using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;
using MittClick.Models.ViewModels;

namespace MittClick.Controllers
{
    public class ProjectController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private readonly MittClickDbContext dbContext;
        private readonly IImageService imageService;

        public ProjectController(UserManager<User> userManager, SignInManager<User> signInManager, MittClickDbContext dbContext, IImageService imageService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.imageService = imageService;
        }

        public IActionResult Index()
        {
            var allProjects = dbContext.Projects.ToList(); // Hämta alla projekt från databasen
            return View(allProjects); // Skicka med alla projekt till vyn för att visa dem
        }

        public IActionResult All()
        {
            var allProjects = dbContext.Projects.ToList(); // Hämta alla projekt från databasen
            return View(allProjects);
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

            // Ny logik för att filtrera profiler
            var isUserAuthenticated = User.Identity.IsAuthenticated;
            project.PartOfProjects = project.PartOfProjects
                .Where(pp => isUserAuthenticated || !pp.User.Profile.PrivateProfile)
                .ToList();


            var currentUser = userManager.GetUserAsync(User).Result;
            var isCurrentUserPartOfProject = project.PartOfProjects.Any(pp => pp.UId == currentUser?.Id);

            ViewData["IsCurrentUserPartOfProject"] = isCurrentUserPartOfProject;

            var projectLeaderFullName = dbContext.Users
            .Where(u => u.Id == project.UserId)
            .Select(u => u.Profile.FirstName + " " + u.Profile.LastName)
            .FirstOrDefault();

            ViewData["ProjectLeaderFullName"] = projectLeaderFullName;

            var projectLeaderId = dbContext.Projects
            .Where(p => p.ProjectId == projectId)
            .Select(p => p.UserId)
            .FirstOrDefault();

            ViewData["ProjectLeaderUserId"] = projectLeaderId;

            return View("Project", project);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddProjectViewModel addProjectViewModel = new AddProjectViewModel();
            return View(addProjectViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddProjectViewModel addProjectViewModel)
        {
            var currentUser = userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    Models.Project newProject = new Models.Project
                    {
                        Title = addProjectViewModel.Title,
                        Summary = addProjectViewModel.Summary,
                        Description = addProjectViewModel.Description,
                        UserId = currentUser.Id
                    };

                    if (addProjectViewModel.ProjectImage != null && addProjectViewModel.ProjectImage.Length > 0)
                    {
                        var image = new Image
                        {
                            Data = imageService.ConvertToByteArray(addProjectViewModel.ProjectImage)
                        };

                        dbContext.Images.Add(image);
                        dbContext.SaveChanges();

                        newProject.ProjectImage = image.Data;
                    }
                    dbContext.Projects.Add(newProject);
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
                    return RedirectToAction("Add", "Project");

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

        [HttpGet]
        public IActionResult Edit(int projectId)
        {
            var project = dbContext.Projects.FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
            {
                return RedirectToAction("Index");
            }

            var editProjectViewModel = new EditProjectViewModel
            {
                ProjectId = projectId,
                Title = project.Title,
                Summary = project.Summary,
                Description = project.Description,
            };

            return View("Edit", editProjectViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditProjectViewModel editedProject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProject = dbContext.Projects.FirstOrDefault(p => p.ProjectId == editedProject.ProjectId);

                    if (existingProject != null)
                    {
                        existingProject.Title = editedProject.Title;
                        existingProject.Summary = editedProject.Summary;
                        existingProject.Description = editedProject.Description;

                        if (editedProject.ProjectImage != null && editedProject.ProjectImage.Length > 0)
                        {
                            var image = new Image
                            {
                                Data = imageService.ConvertToByteArray(editedProject.ProjectImage)
                            };

                            dbContext.Images.Add(image);
                            dbContext.SaveChanges();

                            existingProject.ProjectImage = image.Data;
                        }

                        dbContext.SaveChanges();

                        return RedirectToAction("Project", "Project", new { projectId = existingProject.ProjectId });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                    return RedirectToAction("Error", "Home"); // Redirect till en sida som visar felmeddelandet för användaren
                }
            }

            return View("Edit", editedProject);
        }
    }
}
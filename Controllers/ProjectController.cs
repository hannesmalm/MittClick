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
            return View();
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
                    // Lägg till Projektbild
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
                    // Spara till din databas, antingen via en DbContext eller annan datahanteringsmetod
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

        // Hämta användarinformation baserat på userId och rendera profilsidan


        public IActionResult ViewUserProfile(string userId)
        {
            Console.WriteLine("User ID received: " + userId);

            if (string.IsNullOrEmpty(userId))
            {
                var currentUser = userManager.GetUserAsync(User).Result;
                if (currentUser == null)
                {
                    // Hantera scenariot där ingen inloggad användare finns
                    return NotFound();
                }
                userId = currentUser.Id;
            }

            var userProfile = dbContext.Profiles.FirstOrDefault(p => p.UserId == userId);
            if (userProfile == null)
            {
                Console.WriteLine("User profile not found for user ID: " + userId);
                return NotFound();
            }

            return View(userProfile);
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
            Console.WriteLine(editedProject.ProjectId);
            Console.WriteLine(editedProject.Title);
            Console.WriteLine(editedProject.Summary);
            Console.WriteLine(editedProject.Description);


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
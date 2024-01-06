using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> userManager;
        private readonly MittClickDbContext dbContext;
        private readonly IImageService imageService;

        public ProfileController(UserManager<User> userManager, MittClickDbContext dbContext, IImageService imageService)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.imageService = imageService;
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
                    return RedirectToAction("Create");
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
        public IActionResult Create()
        {
            CreateProfileViewModel createProfileViewModel = new CreateProfileViewModel();
            return View(createProfileViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateProfileViewModel createProfileViewModel)
        {
            var currentUser = userManager.GetUserAsync(User).Result;

            if (ModelState.IsValid)
            {

                Profile newProfile = new Profile
                {
                    User = currentUser,
                    UserName = currentUser.UserName,
                    UserId = currentUser.Id,
                    FirstName = createProfileViewModel.FirstName,
                    LastName = createProfileViewModel.LastName,
                    PrivateProfile = createProfileViewModel.PrivateProfile,
                    Information = createProfileViewModel.Information,
                    
                };

                // Profilbild
                if (createProfileViewModel.ProfileImage != null && createProfileViewModel.ProfileImage.Length > 0)
                {
                    var image = new Image
                    {
                        Data = imageService.ConvertToByteArray(createProfileViewModel.ProfileImage)
                    };

                    dbContext.Images.Add(image);
                    dbContext.SaveChanges();

                    newProfile.ProfileImage = image.Data;
                }

                dbContext.Profiles.Add(newProfile);
                

                // Skills
                if (createProfileViewModel.Skills == null)
                {
                    createProfileViewModel.Skills = new List<Skill>();
                }
                foreach (var skill in createProfileViewModel.Skills)
                {
                    newProfile.Skills.Add(new Skill { Name = skill.Name, ProfileId = newProfile.ProfileId });
                }

                // Kontaktinfo
                if (createProfileViewModel.ContactInfos == null)
                {
                    createProfileViewModel.ContactInfos = new List<ContactInfo>();
                }
                foreach (var contact in createProfileViewModel.ContactInfos)
                {
                    newProfile.ContactInfos.Add(new ContactInfo { Type = contact.Type, Info = contact.Info, ProfileId = newProfile.ProfileId });
                }

				// Utbildning
				if (createProfileViewModel.Educations == null)
				{
					createProfileViewModel.Educations = new List<Education>();
				}
				foreach (var education in createProfileViewModel.Educations)
				{
					newProfile.Educations.Add(new Education { School = education.School, Type = education.Type, From = education.From, To = education.To, ProfileId = newProfile.ProfileId });
				}

                //Arbetserfarenheter

                if (createProfileViewModel.WorkExperiences == null)
                {
                    createProfileViewModel.WorkExperiences = new List<WorkExperience>();
                }
                foreach (var workexperience in createProfileViewModel.WorkExperiences)
                {
                    newProfile.WorkExperiences.Add(new WorkExperience { Workplace = workexperience.Workplace, Role = workexperience.Role, From = workexperience.From, To = workexperience.To, ProfileId = newProfile.ProfileId });
                }

                dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(createProfileViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var userProfile = dbContext.Profiles
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == currentUser.Id);

            EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PrivateProfile = userProfile.PrivateProfile,
                Information = userProfile.Information,

                // Lämna ProfileImage tomt för att undvika överföring av bilddata till klienten
            };

            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel editProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                try
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
                       

                        // Profilbildsuppdatering endast om en ny bild laddas upp
                        if (editProfileViewModel.ProfileImage != null && editProfileViewModel.ProfileImage.Length > 0)
                        {
                            var image = new Image
                            {
                                Data = imageService.ConvertToByteArray(editProfileViewModel.ProfileImage)
                            };

                            dbContext.Images.Add(image);
                            dbContext.SaveChanges();

                            userProfile.ProfileImage = image.Data;
                        }

                        dbContext.SaveChanges();
                        Console.WriteLine("Profilen uppdaterades framgångsrikt.");
                    }
                    else
                    {
                        Console.WriteLine("Användaren har ingen profil.");
                        return NotFound();
                    }

                    return RedirectToAction("MyProfile", "Profile");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                    return RedirectToAction("Error", "Home"); // Redirect till en sida som visar felmeddelandet för användaren
                }
            }
            else
            {
                Console.WriteLine("ModelState är inte giltig.");
                return View(editProfileViewModel);
            }
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}

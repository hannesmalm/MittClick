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

        public IActionResult Profile(string userId)
        {
            var profileEntity = dbContext.Profiles
                                         .Include(p => p.User)
                                         .FirstOrDefault(p => p.UserId == userId);
            if (profileEntity != null)
            {
                var userProjects = GetUserProjects(userId);
                var userProfileViewModel = new UserProfileViewModel
                {
                    UserName = profileEntity.User.UserName,
                    UserId = profileEntity.UserId,
                    HasProfile = true,
                    ProfileId = profileEntity.ProfileId,
                    FirstName = profileEntity.FirstName,
                    LastName = profileEntity.LastName,
                    PrivateProfile = profileEntity.PrivateProfile,
                    Information = profileEntity.Information,
                    ProfileImage = profileEntity.ProfileImage,

                    UserProjects = userProjects
                };


                return View("Profile", userProfileViewModel);
            }
            else
            {
                return View("EmptyProfile");
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
                .Include(p => p.ContactInfos)
                .Include(p => p.Skills)
                .Include(p => p.Educations)
                .Include(p => p.WorkExperiences)
                .FirstOrDefault(p => p.UserId == currentUser.Id);

            if (userProfile != null)
            {

                EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
                {
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    PrivateProfile = userProfile.PrivateProfile,
                    Information = userProfile.Information,
                    // Populate additional collections
                    ContactInfos = userProfile.ContactInfos.Select(ci => new ContactInfo { Type = ci.Type, Info = ci.Info }).ToList(),
                    Skills = userProfile.Skills.Select(s => new Skill { Name = s.Name }).ToList(),
                    Educations = userProfile.Educations.Select(e => new Education { School = e.School, Type = e.Type, From = e.From, To = e.To }).ToList(),
                    WorkExperiences = userProfile.WorkExperiences.Select(we => new WorkExperience { Workplace = we.Workplace, Role = we.Role, From = we.From, To = we.To }).ToList()
                };


                return View(editProfileViewModel);
            }
            else
            {
                Console.WriteLine("Användaren har ingen profil.");
                return NotFound();
            }
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
                        .Include(p => p.ContactInfos)
                        .Include(p => p.Skills)
                        .Include(p => p.Educations)
                        .Include(p => p.WorkExperiences)
                        .FirstOrDefault(p => p.UserId == currentUser.Id);

                    if (userProfile != null)
                    {
                        userProfile.FirstName = editProfileViewModel.FirstName;
                        userProfile.LastName = editProfileViewModel.LastName;
                        userProfile.PrivateProfile = editProfileViewModel.PrivateProfile;
                        userProfile.Information = editProfileViewModel.Information;

                        // Update ProfileImage if a new image is uploaded
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
                        //UpdateContactInfos(userProfile, editProfileViewModel.ContactInfos);
                        UpdateSkills(userProfile, editProfileViewModel.Skills);
                        //UpdateEducations(userProfile, editProfileViewModel.Educations);
                        //UpdateWorkExperiences(userProfile, editProfileViewModel.WorkExperiences);

                        dbContext.SaveChanges();
                        ViewBag.ContactInfos = userProfile.ContactInfos;
                        ViewBag.Skills = userProfile.Skills;
                        ViewBag.Educations = userProfile.Educations;
                        ViewBag.WorkExperiences = userProfile.WorkExperiences;
                        return RedirectToAction("Profile", "Profile", new { userId = currentUser.Id });
                    }
                    else
                    {
                        Console.WriteLine("Användaren har ingen profil.");
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                Console.WriteLine("hehehehhehehehe");
                
                return View(editProfileViewModel);
            }
        }
        private void UpdateSkills(Profile userProfile, ICollection<Skill> newSkills)
        {
            // Skapa en kopia av befintliga färdigheter
            var existingSkillsCopy = new List<Skill>(userProfile.Skills);

            // Ta bort alla befintliga färdigheter
            foreach (var skillToRemove in existingSkillsCopy)
            {
                userProfile.Skills.Remove(skillToRemove);
            }

            // Lägg till alla nya färdigheter från formuläret
            foreach (var skill in newSkills)
            {
                userProfile.Skills.Add(new Skill { Name = skill.Name, ProfileId = userProfile.ProfileId });
            }
        }

        private void UpdateContactInfos(Profile userProfile, ICollection<ContactInfo> newContactInfos)
        {
            var existingContactInfosCopy = new List<ContactInfo>(userProfile.ContactInfos);

            foreach (var contactInfoToRemove in existingContactInfosCopy)
            {
                userProfile.ContactInfos.Remove(contactInfoToRemove);
            }

            foreach (var contactInfo in newContactInfos)
            {
                userProfile.ContactInfos.Add(new ContactInfo
                {
                    Type = contactInfo.Type,
                    Info = contactInfo.Info,
                    ProfileId = userProfile.ProfileId
                });
            }
        }

        private void UpdateEducations(Profile userProfile, ICollection<Education> newEducations)
        {
            var existingEducationsCopy = new List<Education>(userProfile.Educations);

            foreach (var educationToRemove in existingEducationsCopy)
            {
                userProfile.Educations.Remove(educationToRemove);
            }

            foreach (var education in newEducations)
            {
                userProfile.Educations.Add(new Education
                {
                    School = education.School,
                    Type = education.Type,
                    From = education.From,
                    To = education.To,
                    ProfileId = userProfile.ProfileId
                });
            }
        }

        private void UpdateWorkExperiences(Profile userProfile, ICollection<WorkExperience> newWorkExperiences)
        {
            Console.WriteLine("hoppelihopp");
            // Skapa en kopia av befintliga arbetslivserfarenheter
            var existingWorkExperiencesCopy = new List<WorkExperience>(userProfile.WorkExperiences);

            // Ta bort alla befintliga arbetslivserfarenheter
            foreach (var workExperienceToRemove in existingWorkExperiencesCopy)
            {
                userProfile.WorkExperiences.Remove(workExperienceToRemove);
            }

            // Lägg till alla nya arbetslivserfarenheter från formuläret
            foreach (var workExperienceViewModel in newWorkExperiences)
            {
                userProfile.WorkExperiences.Add(new WorkExperience
                {
                    Workplace = workExperienceViewModel.Workplace,
                    Role = workExperienceViewModel.Role,
                    From = workExperienceViewModel.From,
                    To = workExperienceViewModel.To,
                    ProfileId = userProfile.ProfileId
                });
            }
        }


        private List<Project> GetUserProjects(string userId)
        {
            return dbContext.PartOfProjects
                           .Where(pop => pop.UId == userId)
                           .Include(pop => pop.Project)
                           .Select(pop => pop.Project)
                           .ToList();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}

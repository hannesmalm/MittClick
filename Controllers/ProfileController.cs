using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MittClick.Models;
using MittClick.Models.ViewModels;

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
                                         .Include(p => p.Skills)
                                         .Include(p => p.ContactInfos)
                                         .Include(p => p.Educations)
                                         .Include(p => p.WorkExperiences)
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
                    UserProjects = userProjects,
                    Skills = profileEntity.Skills.ToList(),
                    ContactInfos = profileEntity.ContactInfos.ToList(),
                    Educations = profileEntity.Educations.ToList(),
                    WorkExperiences = profileEntity.WorkExperiences.ToList(),

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
                    //newProfile.Skills.Add(new Skill { Name = skill.Name, ProfileId = newProfile.ProfileId });
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

            if (userProfile != null)
            {

                EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
                {
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    PrivateProfile = userProfile.PrivateProfile,
                    Information = userProfile.Information
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

                        dbContext.SaveChanges();
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

        [HttpGet]
        public async Task<IActionResult> UpdateEducation()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userProfile = dbContext.Profiles.Include(p => p.User).FirstOrDefault(p => p.UserId == currentUser.Id);
            var Educations = userProfile?.Educations.ToList();

            var updateEducationViewModel = new UpdateEducationViewModel()
            {
                Profile = userProfile,
                Educations = Educations
            };

            return View(updateEducationViewModel);
        }

        public async Task<IActionResult> AddEducation(string school, string type, int from, int to)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userProfile = dbContext.Profiles
                              .Include(p => p.User)
                              .FirstOrDefault(p => p.UserId == currentUser.Id);

            Education education = new Education
            {
                School = school,
                Type = type,
                From = from,
                To = to,
                ProfileId = userProfile.ProfileId,
            };
            try
            {
                dbContext.Educations.Add(education);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("UpdateEducation", "Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEducation(int id)
        {
            try
            {
                Education education = dbContext.Educations.FirstOrDefault(e => e.Id == id);
                Console.WriteLine(id);

                if (education != null)
                {
                    dbContext.Educations.Remove(education);
                    dbContext.SaveChanges();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("UpdateEducation", "Profile");
        }


        //     >>METODER FÖR ATT HANTERA SKILLS<<

        [HttpGet]
        public async Task<IActionResult> UpdateSkills()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userProfile = dbContext.Profiles
                              .Include(p => p.User)
                              .FirstOrDefault(p => p.UserId == currentUser.Id);
            var skills = userProfile?.Skills;

            var updateSkillViewModel = new UpdateSkillViewModel
            {
                Profile = userProfile,
                Skills = skills
            };

            return View(updateSkillViewModel);
        }

        [HttpPost]
        public IActionResult DeleteSkill(int id)
        {
            try
            {
                Skill skill = dbContext.Skills.Find(id);
                if (skill != null)
                {
                    dbContext.Skills.Remove(skill);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("UpdateSkills", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill(string name)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userProfile = dbContext.Profiles
                              .Include(p => p.User)
                              .FirstOrDefault(p => p.UserId == currentUser.Id);

            //Kollar om objektet redan finns
            bool skillExists = dbContext.Skills
                   .Any(s => s.Name.ToUpper() == name.ToUpper()
                             && s.ProfileId == userProfile.ProfileId);

            if (skillExists)
            {
                ModelState.AddModelError("Name", "Färdigheten finns redan.");

                // Förbereder modellen för att skicka tillbaka till vyn
                var updateSkillViewModel = new UpdateSkillViewModel
                {
                    Profile = userProfile,
                    Skills = userProfile?.Skills
                };

                return View("UpdateSkills", updateSkillViewModel);
            }

            Skill skill = new Skill
            {
                Name = name,
                ProfileId = userProfile.ProfileId,
            };
            try
            {
                dbContext.Skills.Add(skill);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("UpdateSkills", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateContactInfo()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userProfile = dbContext.Profiles
                              .Include(p => p.User)
                              .FirstOrDefault(p => p.UserId == currentUser.Id);
            var contactInfos = userProfile?.ContactInfos.ToList();

            var updateContactInfoViewModel = new UpdateContactInfoViewModel
            {
                Profile = userProfile,
                ContactInfos = contactInfos
            };

            return View(updateContactInfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddContactInfo(string newContactType, string newContactInfo)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userProfile = dbContext.Profiles
                              .Include(p => p.User)
                              .FirstOrDefault(p => p.UserId == currentUser.Id);

            bool contactExists = dbContext.Contacts
                .Any(s => s.Type.ToUpper() == newContactType.ToUpper()
                 && s.Info.ToUpper() == newContactInfo.ToUpper()
                 && s.ProfileId == userProfile.ProfileId);

            if (contactExists)
            {
                Response.StatusCode = 400; // Bad Request
                return Content("Denna kontaktinformation finns redan.");
            }

            ContactInfo contactInfo = new ContactInfo
            {
                Type = newContactType,
                Info = newContactInfo,
                ProfileId = userProfile.ProfileId
            };
            try
            {
                dbContext.Contacts.Add(contactInfo);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("UpdateContactInfo", "Profile");
        }

            public IActionResult DeleteContactInfo(int id)
            {
                try
                {
                    ContactInfo contactInfo = dbContext.Contacts.Find(id);
                    if (contactInfo != null)
                    {
                        dbContext.Contacts.Remove(contactInfo);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return RedirectToAction("UpdateContactInfo", "Profile");
            }

        private List<Project> GetUserProjects(string userId)
        {
            return dbContext.PartOfProjects
                           .Where(pop => pop.UId == userId)
                           .Include(pop => pop.Project)
                           .Select(pop => pop.Project)
                           .ToList();
        }

        // Arbetserfarenheter
        [HttpGet]
        public async Task<IActionResult> UpdateWorkExperience()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var userProfile = dbContext.Profiles
                              .Include(p => p.User)
                              .FirstOrDefault(p => p.UserId == currentUser.Id);
            var workExperiences = userProfile.WorkExperiences.ToList();
            var updateWorkExperienceViewModel = new UpdateWorkExperienceViewModel
            {
                Profile = userProfile,
                WorkExperiences = workExperiences
            };
            return View(updateWorkExperienceViewModel);
        }

        [HttpPost]
        public IActionResult DeleteWorkExperience(int id)
        {
            try
            {
                WorkExperience workExperience = dbContext.WorkExperiences.Find(id);
                if (workExperience != null)
                {
                    dbContext.WorkExperiences.Remove(workExperience);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // Lägg eventuellt till ytterligare felhantering här
                return BadRequest("Något gick fel vid borttagning av arbetslivserfarenheten.");
            }

            return RedirectToAction("UpdateWorkExperience", "Profile");
        }

		[HttpPost]
		public async Task<IActionResult> AddWorkExperience(string workplace, string role, int from, int? to)
		{
			try
			{
				// Hämta aktuell användare
				var currentUser = await userManager.GetUserAsync(User);

				// Hämta användarens profil inklusive arbetslivserfarenheter
				var userProfile = dbContext.Profiles
									  .Include(p => p.User)
									  .Include(p => p.WorkExperiences)
									  .FirstOrDefault(p => p.UserId == currentUser.Id);

				// Om "Från" är större än eller lika med "Till", lägg till valideringsfel och returnera till vyn
				
                if (to != null && from >= to)
				{
					ModelState.AddModelError("To", "Fråndatumet måste vara lägre än slutdatumet");

					// Skapa en UpdateWorkExperienceViewModel och skicka tillbaka till vyn med valideringsfel
					var updateWorkExperienceViewModel = new UpdateWorkExperienceViewModel
					{
						Profile = userProfile,
						WorkExperiences = userProfile.WorkExperiences
					};

					return View("UpdateWorkExperience", updateWorkExperienceViewModel);
				}

				// Skapa en ny arbetslivserfarenhet och lägg till den i användarens profil
				WorkExperience newWorkExperience = new WorkExperience
				{
					Workplace = workplace,
					Role = role,
					From = from,
					To = to,
				};

				userProfile.WorkExperiences.Add(newWorkExperience);

				// Spara ändringarna till databasen
				dbContext.SaveChanges();

				// Återvänd till sidan för att uppdatera arbetslivserfarenheter
				return RedirectToAction("UpdateWorkExperience", "Profile");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				// Lägg eventuellt till ytterligare felhantering här
				return BadRequest("Något gick fel vid läggning till arbetslivserfarenheten.");
			}
		}


		public IActionResult Index()
        {
            return View();
        }
    }
}

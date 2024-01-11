namespace MittClick.Models
{
    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public bool HasProfile { get; set; }
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool PrivateProfile { get; set; }
        public string? Information { get; set; }
        public byte[]? ProfileImage { get; set; }
        public List<ContactInfo> ContactInfos { get; set; }
        public List<Education> Educations { get; set; }
        public List<WorkExperience> WorkExperiences { get; set; }
        public List<Skill> Skills { get; set; }

        public List<Project> UserProjects { get; set; }

        public UserProfileViewModel()
        {
            UserProjects = new List<Project>();
            Skills = new List<Skill>();
            WorkExperiences = new List<WorkExperience>();
            Educations = new List<Education>();
            ContactInfos = new List<ContactInfo>();
        }
    }
}

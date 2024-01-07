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
        public string? Resume { get; set;}

        public List<Project> UserProjects { get; set; }

        public UserProfileViewModel()
        {
            UserProjects = new List<Project>();
        }
    }
}

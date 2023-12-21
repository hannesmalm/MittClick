
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MittClick.Models
{
    public class ProfileContext : IdentityDbContext<User>
    {
        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }

        public DbSet<ProfileViewModel> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

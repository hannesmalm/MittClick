using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MittClick.Models
{
    public class MittClickDbContext : IdentityDbContext<User>
    {
        public MittClickDbContext(DbContextOptions<MittClickDbContext> options) : base(options) { }
        public DbSet<ProfileViewModel> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

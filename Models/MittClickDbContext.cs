using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MittClick.Models
{
    public class MittClickDbContext : IdentityDbContext<User>
    {
        public MittClickDbContext(DbContextOptions<MittClickDbContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill>Skills { get; set; }
        public DbSet<ContactInfo> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

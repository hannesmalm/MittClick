using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MittClick.Models
{
    public class MittClickDbContext : IdentityDbContext<User>
    {
        public MittClickDbContext(DbContextOptions<MittClickDbContext> options) : base(options) { }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.ProfileId)
                .HasPrincipalKey<User>(u => u.Id);
        }
    }
}

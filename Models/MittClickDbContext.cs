using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MittClick.Models
{
    public class MittClickDbContext : IdentityDbContext<User>
    {
        public MittClickDbContext(DbContextOptions<MittClickDbContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Image> Images { get; set; }
        public DbSet<PartOfProject> PartOfProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PartOfProject>()
                .HasKey(pp => new { pp.UId, pp.PId });

            modelBuilder.Entity<PartOfProject>()
                .HasOne(pp => pp.User)
                .WithMany(u => u.PartOfProjects)
                .HasForeignKey(pp => pp.UId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PartOfProject>()
                .HasOne(pp => pp.Project)
                .WithMany(p => p.PartOfProjects)
                .HasForeignKey(pp => pp.PId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => p.UserId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("ApplicationDbContext");
            }
        }
    }
}
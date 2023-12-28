using Microsoft.AspNetCore.Identity;

namespace MittClick.Models
{
    public class DataSeeder
    {
        private readonly UserManager<User> userManager;
        private readonly MittClickDbContext dbContext;

        public DataSeeder(UserManager<User> userManager, MittClickDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public void SeedData()
        {
            string[] firstNames = { "Alice", "Bob", "Charlie", /* ... */ };
            string[] lastNames = { "Anderson", "Brown", "Clark", /* ... */ };

            for (int i = 0; i < 20; i++)
            {
                var userName = $"User{i + 1}";

                var newUser = new User
                {
                    Id = $"UserId{i + 1}",
                    UserName = userName,
                    ProfileId = i + 1
                };

                var password = $"Password{i + 1}";

                var result = userManager.CreateAsync(newUser, password).Result;

                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create user '{userName}'.");
                }

                dbContext.Profiles.Add(new Profile
                {
                    ProfileId = i + 1,
                    FirstName = firstNames[i % firstNames.Length],
                    LastName = lastNames[i % lastNames.Length],
                    PrivateProfile = true,
                    UserId = $"UserId{i + 1}"
                });
            }

            dbContext.SaveChanges();
        }
    }
}

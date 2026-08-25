using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Seeders;
using LeksanaStudio.Common.Helpers;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Application.Seeders
{
    /// <summary>
    /// Seeds initial users from config ("Seeder:Users"), granting each its listed role
    /// codes. No credentials are hardcoded: with nothing configured (e.g. production)
    /// this seeds no user. Idempotent — existing emails are skipped.
    /// </summary>
    public class UserSeeder(
        AppDbContext context,
        IConfiguration configuration,
        ILogger<UserSeeder> logger
    ) : ISeeder
    {
        public int Order => 1;

        public async Task SeedAsync()
        {
            var users = configuration.GetSection("Seeder").Get<SeederOptions>()?.Users ?? [];

            if (users.Length == 0)
            {
                logger.LogInformation("UserSeeder: no seed users configured — skipping.");
                return;
            }

            var seeded = 0;
            foreach (var seed in users)
            {
                if (string.IsNullOrWhiteSpace(seed.Email))
                    continue;

                if (await context.Users.AnyAsync(u => u.Email == seed.Email))
                    continue;

                var user = new User
                {
                    Name = seed.Name,
                    Email = seed.Email,
                    Password = PasswordHash.Hash(seed.Password),
                    CreatedBy = "SEEDER",
                };
                context.Users.Add(user);

                foreach (var roleCode in seed.Roles)
                {
                    var role = await context.Roles.FirstOrDefaultAsync(r => r.Code == roleCode);
                    if (role == null)
                    {
                        logger.LogWarning(
                            "UserSeeder: role '{Role}' not found for {Email} — skipping grant.",
                            roleCode,
                            seed.Email
                        );
                        continue;
                    }

                    context.UserRoles.Add(
                        new UserRole
                        {
                            UserId = user.Id,
                            RoleId = role.Id,
                            CreatedBy = "SEEDER",
                        }
                    );
                }

                seeded++;
            }

            await context.SaveChangesAsync();

            logger.LogInformation(
                "UserSeeder: seeded {Seeded} user(s), skipped {Skipped} (already existed).",
                seeded,
                users.Length - seeded
            );
        }
    }
}

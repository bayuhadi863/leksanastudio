using LeksanaStudio.Application.Interfaces.Seeders;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.Application.Seeders
{
    public class DatabaseSeeder(
        IEnumerable<ISeeder> seeders,
        IConfiguration configuration,
        ILogger<DatabaseSeeder> logger
    )
    {
        public async Task SeedAllAsync()
        {
            var options =
                configuration.GetSection("Seeder").Get<SeederOptions>() ?? new SeederOptions();

            if (!options.Enabled)
            {
                logger.LogInformation(
                    "Database seeding disabled via config (Seeder:Enabled=false). Skipping all seeders."
                );
                return;
            }

            var disabled = new HashSet<string>(
                options.Disabled ?? [],
                StringComparer.OrdinalIgnoreCase
            );

            logger.LogInformation("Starting database seeding...");

            // Explicit Order drives sequence (auto-registration order is not deterministic).
            foreach (var seeder in seeders.OrderBy(s => s.Order))
            {
                var name = seeder.GetType().Name;
                if (disabled.Contains(name))
                {
                    logger.LogInformation("Seeder {Name} disabled via config — skipping.", name);
                    continue;
                }

                await seeder.SeedAsync();
            }

            logger.LogInformation("Database seeding completed.");
        }
    }
}

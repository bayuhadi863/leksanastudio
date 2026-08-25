using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Extensions
{
    public static class MigrationExtensions
    {
        /// <summary>
        /// Applies pending EF Core migrations on startup so the schema is always up to date —
        /// no manual `dotnet ef database update` needed (local or deployed). Controlled by the
        /// optional config flag "Database:AutoMigrate" (defaults to true).
        /// </summary>
        public static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            if (!app.Configuration.GetValue("Database:AutoMigrate", true))
            {
                logger.LogInformation(
                    "Database auto-migration is disabled (Database:AutoMigrate=false). Skipping."
                );
                return;
            }

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            logger.LogInformation("Applying pending database migrations...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");
        }
    }
}

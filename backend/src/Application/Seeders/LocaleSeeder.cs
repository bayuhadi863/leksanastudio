using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Seeders;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Application.Seeders
{
    /// <summary>
    /// The languages the site knows about.
    ///
    /// English is seeded but inactive on purpose. The architecture carries a second
    /// language from the first row of schema — but a language with no content is a
    /// half-translated site, which reads worse than one honest language. It is
    /// switched on when its content exists, not before.
    /// </summary>
    public class LocaleSeeder(AppDbContext context, ILogger<LocaleSeeder> logger) : ISeeder
    {
        private static readonly IReadOnlyList<(
            string Code,
            string Name,
            string NativeName,
            bool IsDefault,
            bool IsActive,
            int Order
        )> _locales =
        [
            ("id", "Bahasa Indonesia", "Bahasa Indonesia", true, true, 1),
            ("en", "Inggris", "English", false, false, 2),
        ];

        public int Order => 4;

        public async Task SeedAsync()
        {
            var seeded = 0;

            foreach (var (code, name, nativeName, isDefault, isActive, order) in _locales)
            {
                if (await context.Locales.AnyAsync(l => l.Code == code && !l.IsDeleted))
                    continue;

                context.Locales.Add(
                    new Locale
                    {
                        Code = code,
                        Name = name,
                        NativeName = nativeName,
                        IsDefault = isDefault,
                        IsActive = isActive,
                        Order = order,
                        CreatedBy = "SEEDER",
                    }
                );
                seeded++;
            }

            await context.SaveChangesAsync();

            logger.LogInformation(
                "LocaleSeeder: seeded {Seeded}, skipped {Skipped} (already existed).",
                seeded,
                _locales.Count - seeded
            );
        }
    }
}

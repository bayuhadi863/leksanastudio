using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Seeders;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Application.Seeders
{
    /// <summary>
    /// The menu catalogue: one row per manageable area of the panel.
    ///
    /// A menu is what a permission attaches to, so this list is also the list of
    /// things a role can be granted. Content menus land here as the CMS grows; a
    /// new module means one line, not a new permission mechanism.
    /// </summary>
    public class MenuSeeder(AppDbContext context, ILogger<MenuSeeder> logger) : ISeeder
    {
        // CustomEvents: comma-separated custom-event codes the menu supports beyond
        // CRUD (null = only CRUD applies).
        private static readonly IReadOnlyList<(
            string Code,
            string Name,
            string? CustomEvents
        )> _menus =
        [
            ("dashboard", "Dasbor", null),

            // Content — what the client actually came to the panel for.
            ("case-study", "Portofolio", null),
            ("note", "Catatan Teknis", null),
            ("service", "Layanan", null),
            ("service-package", "Paket & Harga", null),
            ("project-phase", "Tahapan Sistem", null),
            ("add-on", "Layanan Tambahan", null),
            ("payment-term", "Termin Pembayaran", null),
            ("process-step", "Langkah Proses", null),
            ("vertical", "Halaman Industri", null),
            ("page-copy", "Teks Halaman", null),
            ("page-document", "Halaman Dokumen", null),
            ("media", "Berkas & Gambar", null),

            // Settings — the studio's own, not the client's.
            ("site-profile", "Identitas Situs", null),
            ("locale", "Bahasa", null),
            ("user", "Pengguna", null),
            ("role", "Peran & Akses", null),
        ];

        public int Order => 2;

        public async Task SeedAsync()
        {
            var seeded = 0;
            var updated = 0;

            foreach (var (code, name, customEvents) in _menus)
            {
                var existing = await context.Menus.FirstOrDefaultAsync(m =>
                    m.Code == code && !m.IsDeleted
                );

                if (existing == null)
                {
                    context.Menus.Add(
                        new Menu
                        {
                            Code = code,
                            Name = name,
                            SupportedCustomEvents = customEvents,
                            CreatedBy = "SEEDER",
                        }
                    );
                    seeded++;
                }
                else
                {
                    // Keep the display name and supported custom events in sync
                    // for menus that already exist (e.g. after a rename).
                    var changed = false;

                    if (existing.Name != name)
                    {
                        existing.Name = name;
                        changed = true;
                    }

                    if (existing.SupportedCustomEvents != customEvents)
                    {
                        existing.SupportedCustomEvents = customEvents;
                        changed = true;
                    }

                    if (changed)
                    {
                        existing.UpdatedDate = DateTimeOffset.UtcNow;
                        existing.UpdatedBy = "SEEDER";
                        updated++;
                    }
                }
            }

            await context.SaveChangesAsync();

            logger.LogInformation(
                "MenuSeeder: seeded {Seeded}, updated {Updated} (name/custom events), "
                    + "skipped {Skipped} (already existed).",
                seeded,
                updated,
                _menus.Count - seeded - updated
            );
        }
    }
}

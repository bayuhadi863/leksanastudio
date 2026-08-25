using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Seeders;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Application.Seeders
{
    /// <summary>
    /// Two roles, on purpose.
    ///
    /// <c>super-admin</c> is the studio: it owns accounts, roles and permissions.
    /// <c>editor</c> is the client who actually runs the site day to day — they can
    /// edit content but cannot hand themselves more access. Splitting the two is
    /// what makes handing the panel over safe.
    /// </summary>
    public class RoleSeeder(AppDbContext context, ILogger<RoleSeeder> logger) : ISeeder
    {
        private static readonly IReadOnlyList<(string Code, string Name, string Description, int Order)> _roles =
        [
            (
                "super-admin",
                "Super Admin",
                "Akses penuh: pengguna, peran, dan seluruh konten.",
                1
            ),
            (
                "editor",
                "Editor Konten",
                "Mengelola isi situs. Tidak bisa mengubah pengguna maupun hak akses.",
                2
            ),
        ];

        public int Order => 0;

        public async Task SeedAsync()
        {
            var seeded = 0;
            foreach (var (code, name, description, order) in _roles)
            {
                if (await context.Roles.AnyAsync(r => r.Code == code && !r.IsDeleted))
                {
                    continue;
                }

                context.Roles.Add(
                    new Role
                    {
                        Code = code,
                        Name = name,
                        Description = description,
                        Order = order,
                        CreatedBy = "SEEDER",
                    }
                );
                seeded++;
            }

            await context.SaveChangesAsync();

            logger.LogInformation(
                "RoleSeeder: seeded {Seeded}, skipped {Skipped} (already existed).",
                seeded,
                _roles.Count - seeded
            );
        }
    }
}

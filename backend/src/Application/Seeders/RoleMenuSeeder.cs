using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Seeders;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Application.Seeders
{
    /// <summary>
    /// Which role may do what, per menu.
    ///
    /// Seeded grants are a starting point, not a lock: an existing row is never
    /// overwritten, so permissions edited in the panel survive every redeploy.
    /// </summary>
    public class RoleMenuSeeder(AppDbContext context, ILogger<RoleMenuSeeder> logger) : ISeeder
    {
        /// <summary>A role's grant for one menu, with its per-action (CRUD) permissions.</summary>
        private readonly record struct Grant(
            string MenuCode,
            bool CanView,
            bool CanCreate,
            bool CanUpdate,
            bool CanDelete,
            bool CanVerify,
            string[] CustomEvents
        );

        private static Grant Full(string code) => new(code, true, true, true, true, false, []);

        private static Grant ViewOnly(string code) =>
            new(code, true, false, false, false, false, []);

        /// <summary>
        /// Menus that manage the panel itself — studio-only, never the client's.
        ///
        /// <c>site-profile</c> is in here on purpose: the WhatsApp number and the
        /// email address are the channel every lead arrives through, and a typo
        /// there costs more than a typo anywhere else on the site.
        /// </summary>
        private static readonly string[] _adminMenuCodes = ["user", "role", "locale", "site-profile"];

        /// <summary>Content menus. Editors get these in full — this is what they came for.</summary>
        private static readonly string[] _contentMenuCodes =
        [
            "case-study",
            "note",
            "service",
            "service-package",
            "project-phase",
            "add-on",
            "payment-term",
            "process-step",
            "vertical",
            "page-copy",
            "page-document",
            "media",
        ];

        private static readonly IReadOnlyDictionary<string, Grant[]> _roleMenuMap = new Dictionary<
            string,
            Grant[]
        >
        {
            ["super-admin"] =
            [
                ViewOnly("dashboard"),
                .. _adminMenuCodes.Select(Full),
                .. _contentMenuCodes.Select(Full),
            ],
            // No user/role access on purpose: an editor manages content, not who
            // may log in. That boundary is the reason the panel can be handed over.
            ["editor"] = [ViewOnly("dashboard"), .. _contentMenuCodes.Select(Full)],
        };

        /// <summary>Menu each role lands on after login, when it has access to it.</summary>
        private static readonly IReadOnlyDictionary<string, string> _defaultMenuByRole =
            new Dictionary<string, string>
            {
                ["super-admin"] = "dashboard",
                ["editor"] = "dashboard",
            };

        public int Order => 3;

        public async Task SeedAsync()
        {
            var seeded = 0;
            var skipped = 0;
            var notFound = 0;

            foreach (var (roleCode, grants) in _roleMenuMap)
            {
                var role = await context.Roles.FirstOrDefaultAsync(r =>
                    r.Code == roleCode && !r.IsDeleted
                );
                if (role is null)
                {
                    logger.LogWarning(
                        "Seed RoleMenu: role '{RoleCode}' not found — skipping.",
                        roleCode
                    );
                    notFound += grants.Length;
                    continue;
                }

                foreach (var grant in grants)
                {
                    var menu = await context.Menus.FirstOrDefaultAsync(m =>
                        m.Code == grant.MenuCode && !m.IsDeleted
                    );
                    if (menu is null)
                    {
                        logger.LogWarning(
                            "Seed RoleMenu: menu '{MenuCode}' not found — skipping.",
                            grant.MenuCode
                        );
                        notFound++;
                        continue;
                    }

                    var customEventCodes =
                        grant.CustomEvents.Length > 0 ? string.Join(",", grant.CustomEvents) : null;

                    var existingRow = await context.RoleMenus.FirstOrDefaultAsync(rm =>
                        rm.RoleId == role.Id && rm.MenuId == menu.Id && !rm.IsDeleted
                    );

                    if (existingRow is not null)
                    {
                        // Don't override manual CRUD edits, but ensure newly-declared
                        // custom-event grants land on redeploy.
                        if (
                            customEventCodes is not null
                            && existingRow.CustomEventCodes != customEventCodes
                        )
                        {
                            existingRow.CustomEventCodes = customEventCodes;
                            existingRow.UpdatedDate = DateTimeOffset.UtcNow;
                            existingRow.UpdatedBy = "SEEDER";
                        }
                        skipped++;
                        continue;
                    }

                    context.RoleMenus.Add(
                        new RoleMenu
                        {
                            RoleId = role.Id,
                            MenuId = menu.Id,
                            CanView = grant.CanView,
                            CanCreate = grant.CanCreate,
                            CanUpdate = grant.CanUpdate,
                            CanDelete = grant.CanDelete,
                            CanVerify = grant.CanVerify,
                            CustomEventCodes = customEventCodes,
                            CreatedBy = "SEEDER",
                        }
                    );
                    seeded++;
                }

                await SetDefaultMenuAsync(role);
            }

            await context.SaveChangesAsync();

            logger.LogInformation(
                "RoleMenuSeeder: seeded {Seeded}, skipped {Skipped} (already existed), "
                    + "{NotFound} skipped (role/menu not found).",
                seeded,
                skipped,
                notFound
            );
        }

        // Only fills a blank default — a landing menu chosen in the panel stays put.
        private async Task SetDefaultMenuAsync(Role role)
        {
            if (role.DefaultMenuId is not null)
                return;

            if (!_defaultMenuByRole.TryGetValue(role.Code ?? string.Empty, out var menuCode))
                return;

            var menu = await context.Menus.FirstOrDefaultAsync(m =>
                m.Code == menuCode && !m.IsDeleted
            );
            if (menu is null)
                return;

            role.DefaultMenuId = menu.Id;
            role.UpdatedDate = DateTimeOffset.UtcNow;
            role.UpdatedBy = "SEEDER";
        }
    }
}

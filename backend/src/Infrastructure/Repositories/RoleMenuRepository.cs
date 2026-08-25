using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Common.Repositories;
using LeksanaStudio.Domain.Entities;
using LeksanaStudio.Infrastructure.DataContext;

namespace LeksanaStudio.Infrastructure.Repositories
{
    public class RoleMenuRepository : BaseRepository<RoleMenu>, IRoleMenuRepository
    {
        public RoleMenuRepository(AppDbContext context)
            : base(context) { }

        public async Task<IEnumerable<RoleMenu>> GetByRoleIdAsync(Guid roleId)
        {
            return await _dbSet
                .Where(rm => !rm.IsDeleted && rm.RoleId == roleId)
                .Include(rm => rm.Menu)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoleMenu>> GetUserRoleMenusAsync(
            Guid userId,
            Guid? activeRoleId
        )
        {
            var roleId = await ResolveActiveRoleIdAsync(userId, activeRoleId);
            if (roleId is null)
                return [];

            return await _dbSet
                .Where(rm => !rm.IsDeleted && rm.RoleId == roleId.Value)
                .Include(rm => rm.Menu)
                .Where(rm => rm.Menu != null && !rm.Menu.IsDeleted)
                .ToListAsync();
        }

        public async Task<MenuPermission> GetUserPermissionAsync(
            Guid userId,
            string menuCode,
            Guid? activeRoleId
        )
        {
            var roleId = await ResolveActiveRoleIdAsync(userId, activeRoleId);
            if (roleId is null)
                return MenuPermission.None;

            var rows = await _dbSet
                .Where(rm =>
                    !rm.IsDeleted
                    && rm.RoleId == roleId.Value
                    && rm.Menu != null
                    && !rm.Menu.IsDeleted
                    && rm.Menu.Code == menuCode
                )
                .Select(rm => new
                {
                    rm.CanView,
                    rm.CanCreate,
                    rm.CanUpdate,
                    rm.CanDelete,
                    rm.CanVerify,
                    rm.CustomEventCodes,
                })
                .ToListAsync();

            if (rows.Count == 0)
                return MenuPermission.None;

            var customEvents = rows.SelectMany(r => ParseCustomEvents(r.CustomEventCodes))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return new MenuPermission(
                rows.Any(r => r.CanView),
                rows.Any(r => r.CanCreate),
                rows.Any(r => r.CanUpdate),
                rows.Any(r => r.CanDelete),
                rows.Any(r => r.CanVerify),
                customEvents
            );
        }

        private static IReadOnlyList<string> ParseCustomEvents(string? raw) =>
            string.IsNullOrWhiteSpace(raw)
                ? []
                : raw.Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                    .ToList();

        /// <summary>
        /// Resolves which single role is active for the user. When
        /// <paramref name="activeRoleId"/> is supplied it must be one of the user's
        /// assigned roles (else <see cref="ForbiddenException"/>); when null, the
        /// user's primary role (lowest Order, then Name) is chosen. Returns null when
        /// the user has no assigned roles.
        /// </summary>
        private async Task<Guid?> ResolveActiveRoleIdAsync(Guid userId, Guid? activeRoleId)
        {
            var assigned = await _context
                .Set<UserRole>()
                .Where(ur => !ur.IsDeleted && ur.UserId == userId)
                .Where(ur => ur.Role != null && !ur.Role.IsDeleted)
                .Select(ur => new
                {
                    ur.RoleId,
                    ur.Role!.Order,
                    ur.Role.Name,
                })
                .ToListAsync();

            if (assigned.Count == 0)
                return null;

            if (activeRoleId.HasValue)
            {
                if (assigned.All(a => a.RoleId != activeRoleId.Value))
                    throw new ForbiddenException("Peran aktif tidak valid");
                return activeRoleId.Value;
            }

            return assigned
                .OrderBy(a => a.Order ?? int.MaxValue)
                .ThenBy(a => a.Name)
                .First()
                .RoleId;
        }
    }
}

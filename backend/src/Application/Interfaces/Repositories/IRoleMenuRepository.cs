using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Interfaces.Repositories
{
    public interface IRoleMenuRepository : IBaseRepository<RoleMenu>
    {
        Task<IEnumerable<RoleMenu>> GetByRoleIdAsync(Guid roleId);

        /// <summary>
        /// Role-menu grants (with Menu) for the user's <b>active</b> role. When
        /// <paramref name="activeRoleId"/> is null the user's primary role (lowest
        /// Order) is used; when it is a role not assigned to the user a
        /// <c>ForbiddenException</c> is thrown.
        /// </summary>
        Task<IEnumerable<RoleMenu>> GetUserRoleMenusAsync(Guid userId, Guid? activeRoleId);

        /// <summary>
        /// CRUD/verify permission for one menu code, scoped to the user's active
        /// role. See <see cref="GetUserRoleMenusAsync"/> for active-role resolution.
        /// </summary>
        Task<MenuPermission> GetUserPermissionAsync(
            Guid userId,
            string menuCode,
            Guid? activeRoleId
        );
    }
}

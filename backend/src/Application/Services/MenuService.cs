using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Menu;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Services
{
    public class MenuService
        : BaseCrudService<Menu, MenuDTO, MenuParam, MenuPaginationDTO, MenuPaginationParam>,
            IMenuService
    {
        private readonly IRoleMenuRepository _roleMenuRepository;

        public MenuService(
            IMenuRepository menuRepository,
            IRoleMenuRepository roleMenuRepository,
            ICurrentUserService currentUserService
        )
            : base(menuRepository, currentUserService)
        {
            _roleMenuRepository = roleMenuRepository;
        }

        protected override IQueryable<Menu> ApplyFilter(
            IQueryable<Menu> query,
            MenuPaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
                query = query.Where(m =>
                    (m.Code != null && EF.Functions.ILike(m.Code, $"%{param.Search}%"))
                    || (m.Name != null && EF.Functions.ILike(m.Name, $"%{param.Search}%"))
                );

            if (!string.IsNullOrWhiteSpace(param.Code))
                query = query.Where(m => m.Code != null && EF.Functions.ILike(m.Code, $"%{param.Code}%"));

            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(m => m.Name != null && EF.Functions.ILike(m.Name, $"%{param.Name}%"));

            return query;
        }

        public async Task<IEnumerable<MenuDTO>> GetUserAccessibleMenusAsync()
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                return [];

            var roleMenus = await _roleMenuRepository.GetUserRoleMenusAsync(
                userId.Value,
                _currentUserService.ActiveRoleId
            );

            // A user may hold several roles; merge (OR) the flags per menu, then keep
            // only menus the user can at least view.
            return roleMenus
                .Where(rm => rm.Menu != null)
                .GroupBy(rm => rm.MenuId)
                .Select(g =>
                {
                    var menu = g.First().Menu!;
                    return new MenuDTO
                    {
                        Id = menu.Id,
                        Code = menu.Code,
                        Name = menu.Name,
                        CanView = g.Any(x => x.CanView),
                        CanCreate = g.Any(x => x.CanCreate),
                        CanUpdate = g.Any(x => x.CanUpdate),
                        CanDelete = g.Any(x => x.CanDelete),
                        // Verify only counts on menus that declare support for it.
                        CanVerify =
                            g.Any(x => x.CanVerify)
                            && (menu.SupportedCustomEvents ?? string.Empty)
                                .Split(
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries
                                        | StringSplitOptions.TrimEntries
                                )
                                .Contains("verify", StringComparer.OrdinalIgnoreCase),
                        // Granted custom events, gated by the menu's declared support.
                        CustomEvents = g.SelectMany(x =>
                                (x.CustomEventCodes ?? string.Empty).Split(
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries
                                        | StringSplitOptions.TrimEntries
                                )
                            )
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .Intersect(
                                (menu.SupportedCustomEvents ?? string.Empty).Split(
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries
                                        | StringSplitOptions.TrimEntries
                                ),
                                StringComparer.OrdinalIgnoreCase
                            )
                            .ToList(),
                        CreatedDate = menu.CreatedDate,
                        UpdatedDate = menu.UpdatedDate,
                        CreatedBy = menu.CreatedBy,
                        UpdatedBy = menu.UpdatedBy,
                    };
                })
                .Where(m => m.CanView)
                .ToList();
        }
    }
}

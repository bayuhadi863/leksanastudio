using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Menu;
using LeksanaStudio.Application.DTOs.Role;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Services
{
    public class RoleService
        : BaseCrudService<Role, RoleDTO, RoleParam, RolePaginationDTO, RolePaginationParam>,
            IRoleService
    {
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IMenuRepository _menuRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IRoleMenuRepository roleMenuRepository,
            IMenuRepository menuRepository,
            ICurrentUserService currentUserService
        )
            : base(roleRepository, currentUserService)
        {
            _roleMenuRepository = roleMenuRepository;
            _menuRepository = menuRepository;
        }

        protected override IQueryable<Role> ApplyFilter(
            IQueryable<Role> query,
            RolePaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
                query = query.Where(r =>
                    (r.Code != null && EF.Functions.ILike(r.Code, $"%{param.Search}%"))
                    || (r.Name != null && EF.Functions.ILike(r.Name, $"%{param.Search}%"))
                    || (r.Description != null && EF.Functions.ILike(r.Description, $"%{param.Search}%"))
                );

            if (!string.IsNullOrWhiteSpace(param.Code))
                query = query.Where(r => r.Code != null && EF.Functions.ILike(r.Code, $"%{param.Code}%"));

            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(r => r.Name != null && EF.Functions.ILike(r.Name, $"%{param.Name}%"));

            return query;
        }

        protected override async Task EnrichPaginationAsync(List<RolePaginationDTO> items)
        {
            var ids = items.Select(i => i.Id).ToList();
            var roleMenus = await _roleMenuRepository.GetListAsync(q =>
                q.Where(rm => ids.Contains(rm.RoleId) && !rm.IsDeleted).Include(rm => rm.Menu)
            );
            var lookup = roleMenus
                .GroupBy(rm => rm.RoleId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                        g.Select(rm => new RoleMenuSummaryDTO
                            {
                                MenuId = rm.MenuId,
                                MenuCode = rm.Menu?.Code,
                                MenuName = rm.Menu?.Name,
                            })
                            .ToList()
                );
            foreach (var item in items)
                item.Menus = lookup.GetValueOrDefault(item.Id, []);
        }

        protected override async Task EnrichSingleAsync(Role entity, RoleDTO dto)
        {
            var roleMenus = await _roleMenuRepository.GetByRoleIdAsync(entity.Id);
            dto.Menus = roleMenus.Select(ToRoleMenuDTO).ToList();
        }

        public async Task<IEnumerable<RoleMenuDTO>> GetRoleMenusAsync(Guid roleId)
        {
            var role = await _repository.GetAsync(q => q.Where(r => r.Id == roleId));
            if (role is null)
                throw new NotFoundException($"Role with ID {roleId} not found");

            var roleMenus = await _roleMenuRepository.GetByRoleIdAsync(roleId);
            return roleMenus.Select(ToRoleMenuDTO);
        }

        private static RoleMenuDTO ToRoleMenuDTO(RoleMenu rm) =>
            new()
            {
                Id = rm.Id,
                RoleId = rm.RoleId,
                MenuId = rm.MenuId,
                MenuCode = rm.Menu?.Code,
                MenuName = rm.Menu?.Name,
                CanView = rm.CanView,
                CanCreate = rm.CanCreate,
                CanUpdate = rm.CanUpdate,
                CanDelete = rm.CanDelete,
                CanVerify = rm.CanVerify,
                CustomEvents = ParseCustomEvents(rm.CustomEventCodes),
                CreatedDate = rm.CreatedDate,
                CreatedBy = rm.CreatedBy,
            };

        private static List<string> ParseCustomEvents(string? raw) =>
            string.IsNullOrWhiteSpace(raw)
                ? new List<string>()
                : raw.Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                    .ToList();

        public async Task<PaginationResponse<MenuAssignPickerDTO>> GetMenusForAssignAsync(
            Guid roleId,
            MenuAssignPickerParam param
        )
        {
            var roleMenus = await _roleMenuRepository.GetByRoleIdAsync(roleId);
            var selectedByMenuId = roleMenus
                .GroupBy(rm => rm.MenuId)
                .ToDictionary(g => g.Key, g => g.First());

            var allMenus = await _menuRepository.GetListAsync(q =>
            {
                if (!string.IsNullOrWhiteSpace(param.Search))
                    q = q.Where(m =>
                        (m.Name != null && EF.Functions.ILike(m.Name, $"%{param.Search}%"))
                        || (m.Code != null && EF.Functions.ILike(m.Code, $"%{param.Search}%"))
                    );
                return q;
            });

            var dtos = allMenus.Select(m =>
            {
                selectedByMenuId.TryGetValue(m.Id, out var existing);
                return new MenuAssignPickerDTO
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    IsSelected = existing != null,
                    CanView = existing?.CanView ?? false,
                    CanCreate = existing?.CanCreate ?? false,
                    CanUpdate = existing?.CanUpdate ?? false,
                    CanDelete = existing?.CanDelete ?? false,
                    CanVerify = existing?.CanVerify ?? false,
                    CustomEvents = ParseCustomEvents(m.SupportedCustomEvents),
                };
            });

            bool sortDesc = string.Equals(
                param.SortOrder,
                "desc",
                StringComparison.OrdinalIgnoreCase
            );

            var sorted = sortDesc
                ? dtos.OrderByDescending(m => m.IsSelected)
                    .ThenByDescending(m => m.Name ?? m.Code)
                    .ToList()
                : dtos.OrderByDescending(m => m.IsSelected)
                    .ThenBy(m => m.Name ?? m.Code)
                    .ToList();

            var totalCount = sorted.Count;
            var totalPages =
                totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / param.PageSize);

            var items = sorted
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(
                    (m, i) =>
                    {
                        m.Number = (param.Page - 1) * param.PageSize + i + 1;
                        return m;
                    }
                )
                .ToList();

            return new PaginationResponse<MenuAssignPickerDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPages = totalPages,
            };
        }

        public async Task AssignMenusAsync(Guid roleId, AssignMenusParam param)
        {
            var role = await _repository.GetAsync(q => q.Where(r => r.Id == roleId));
            if (role is null)
                throw new NotFoundException($"Role with ID {roleId} not found");

            await _roleMenuRepository.BeginTransactionAsync();
            try
            {
                var currentRoleMenus = (
                    await _roleMenuRepository.GetByRoleIdAsync(roleId)
                ).ToList();
                var currentByMenuId = currentRoleMenus
                    .GroupBy(rm => rm.MenuId)
                    .ToDictionary(g => g.Key, g => g.First());
                var newItems = (param.Items ?? [])
                    .GroupBy(i => i.MenuId)
                    .ToDictionary(g => g.Key, g => g.First());
                var actor = _currentUserService.UserName ?? "SYSTEM";
                var now = DateTimeOffset.UtcNow;

                // Default menu (post-login landing) must be one of the granted,
                // viewable menus — otherwise the user would land on a 403.
                if (param.DefaultMenuId.HasValue)
                {
                    if (
                        !newItems.TryGetValue(param.DefaultMenuId.Value, out var dm)
                        || !dm.CanView
                    )
                        throw new BadRequestException(
                            "Menu default harus salah satu menu yang diberi akses lihat"
                        );
                }
                role.DefaultMenuId = param.DefaultMenuId;
                await _repository.UpdateAsync(role);

                // A menu can only be granted 'verify' if it declares support for it.
                var allMenus = await _menuRepository.GetListAsync();
                var supportsVerify = allMenus.ToDictionary(
                    m => m.Id,
                    m =>
                        ParseCustomEvents(m.SupportedCustomEvents)
                            .Contains("verify", StringComparer.OrdinalIgnoreCase)
                );
                // Non-verify custom events each menu declares support for.
                var supportedEvents = allMenus.ToDictionary(
                    m => m.Id,
                    m =>
                        ParseCustomEvents(m.SupportedCustomEvents)
                            .Where(c => !c.Equals("verify", StringComparison.OrdinalIgnoreCase))
                            .ToList()
                );

                var toUpdate = new List<RoleMenu>();

                // Remove: grants no longer present in the payload.
                var toRemove = currentRoleMenus
                    .Where(rm => !newItems.ContainsKey(rm.MenuId))
                    .ToList();
                foreach (var rm in toRemove)
                {
                    rm.IsDeleted = true;
                    rm.DeletedDate = now;
                    rm.DeletedBy = actor;
                }
                toUpdate.AddRange(toRemove);

                var toAdd = new List<RoleMenu>();
                foreach (var (menuId, item) in newItems)
                {
                    var effectiveVerify =
                        item.CanVerify && supportsVerify.GetValueOrDefault(menuId);
                    // Keep only requested custom events the menu actually supports.
                    var allowed = supportedEvents.GetValueOrDefault(menuId) ?? [];
                    var effectiveEvents = string.Join(
                        ",",
                        (item.CustomEvents ?? [])
                            .Where(c =>
                                allowed.Contains(c, StringComparer.OrdinalIgnoreCase)
                            )
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                    );
                    var effectiveEventCodes = string.IsNullOrEmpty(effectiveEvents)
                        ? null
                        : effectiveEvents;

                    if (currentByMenuId.TryGetValue(menuId, out var existing))
                    {
                        // Update: only when a flag actually changed.
                        if (
                            existing.CanView != item.CanView
                            || existing.CanCreate != item.CanCreate
                            || existing.CanUpdate != item.CanUpdate
                            || existing.CanDelete != item.CanDelete
                            || existing.CanVerify != effectiveVerify
                            || existing.CustomEventCodes != effectiveEventCodes
                        )
                        {
                            existing.CanView = item.CanView;
                            existing.CanCreate = item.CanCreate;
                            existing.CanUpdate = item.CanUpdate;
                            existing.CanDelete = item.CanDelete;
                            existing.CanVerify = effectiveVerify;
                            existing.CustomEventCodes = effectiveEventCodes;
                            existing.UpdatedDate = now;
                            existing.UpdatedBy = actor;
                            toUpdate.Add(existing);
                        }
                    }
                    else
                    {
                        toAdd.Add(
                            new RoleMenu
                            {
                                RoleId = roleId,
                                MenuId = menuId,
                                CanView = item.CanView,
                                CanCreate = item.CanCreate,
                                CanUpdate = item.CanUpdate,
                                CanDelete = item.CanDelete,
                                CanVerify = effectiveVerify,
                                CustomEventCodes = effectiveEventCodes,
                                CreatedDate = now,
                                CreatedBy = actor,
                            }
                        );
                    }
                }

                if (toUpdate.Count > 0)
                    await _roleMenuRepository.UpdateRangeAsync(toUpdate);
                if (toAdd.Count > 0)
                    await _roleMenuRepository.CreateRangeAsync(toAdd);

                await _roleMenuRepository.CommitTransactionAsync();
            }
            catch
            {
                await _roleMenuRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<StatCountDTO> GetTotalStatAsync()
        {
            var total = await _repository.CountAsync();
            return new StatCountDTO { Count = total, Percentage = total > 0 ? 100 : 0 };
        }

        public async Task<StatCountDTO> GetMenuAccessStatAsync(bool hasAccess)
        {
            var total = await _repository.CountAsync();

            var roleMenus = await _roleMenuRepository.GetListAsync(q =>
                q.Where(rm => !rm.IsDeleted)
            );
            var withAccess = roleMenus.Select(rm => rm.RoleId).Distinct().Count();

            var count = hasAccess ? withAccess : Math.Max(total - withAccess, 0);
            return new StatCountDTO
            {
                Count = count,
                Percentage = total > 0 ? (int)Math.Round((double)count / total * 100) : 0,
            };
        }
    }
}

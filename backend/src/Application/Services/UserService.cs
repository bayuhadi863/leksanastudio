using Microsoft.EntityFrameworkCore;
using LeksanaStudio.Application.DTOs.Role;
using LeksanaStudio.Application.DTOs.User;
using LeksanaStudio.Application.DTOs.UserRole;
using LeksanaStudio.Application.Interfaces.Repositories;
using LeksanaStudio.Application.Interfaces.Services;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Helpers;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Common.Services;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Services
{
    public class UserService
        : BaseCrudService<User, UserDTO, UserParam, UserPaginationDTO, UserPaginationParam>,
            IUserService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            ICurrentUserService currentUserService
        )
            : base(userRepository, currentUserService)
        {
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Hashes the new account's password. Required here rather than in the validator
        /// because the same <see cref="UserParam"/> serves updates, where an empty
        /// password legitimately means "keep the current one".
        /// </summary>
        protected override void OnCreating(User entity, UserParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Password))
                throw new BadRequestException("Kata sandi wajib diisi");

            entity.Password = PasswordHash.Hash(param.Password);
        }

        /// <summary>
        /// Replaces the stored hash only when a new password was supplied. Leaving the
        /// field blank keeps the existing one — the behaviour the edit form promises.
        /// </summary>
        protected override void OnUpdating(User entity, UserParam param)
        {
            if (!string.IsNullOrWhiteSpace(param.Password))
                entity.Password = PasswordHash.Hash(param.Password);
        }

        protected override IQueryable<User> ApplyFilter(
            IQueryable<User> query,
            UserPaginationParam param
        )
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
                query = query.Where(u =>
                    (u.Name != null && EF.Functions.ILike(u.Name, $"%{param.Search}%"))
                    || (u.Email != null && EF.Functions.ILike(u.Email, $"%{param.Search}%"))
                );

            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(u => u.Name != null && EF.Functions.ILike(u.Name, $"%{param.Name}%"));

            if (!string.IsNullOrWhiteSpace(param.Email))
                query = query.Where(u => u.Email != null && EF.Functions.ILike(u.Email, $"%{param.Email}%"));

            return query;
        }

        protected override async Task EnrichPaginationAsync(List<UserPaginationDTO> items)
        {
            var ids = items.Select(i => i.Id).ToList();
            var userRoles = await _userRoleRepository.GetListAsync(q =>
                q.Where(ur => ids.Contains(ur.UserId) && !ur.IsDeleted).Include(ur => ur.Role)
            );
            var lookup = userRoles
                .GroupBy(ur => ur.UserId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                        g.Select(ur => new UserRoleSummaryDTO
                            {
                                RoleId = ur.RoleId,
                                RoleCode = ur.Role?.Code,
                                RoleName = ur.Role?.Name,
                            })
                            .ToList()
                );
            foreach (var item in items)
                item.Roles = lookup.GetValueOrDefault(item.Id, []);
        }

        protected override async Task EnrichSingleAsync(User entity, UserDTO dto)
        {
            var userRoles = await _userRoleRepository.GetByUserIdAsync(entity.Id);
            dto.Roles = userRoles
                .Select(ur => new UserRoleDTO
                {
                    Id = ur.Id,
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,
                    RoleCode = ur.Role?.Code,
                    RoleName = ur.Role?.Name,
                    CreatedDate = ur.CreatedDate,
                    CreatedBy = ur.CreatedBy,
                })
                .ToList();
        }

        public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync(Guid userId)
        {
            var userRoles = await _userRoleRepository.GetByUserIdAsync(userId);
            return userRoles.Select(ur => new UserRoleDTO
            {
                Id = ur.Id,
                UserId = ur.UserId,
                RoleId = ur.RoleId,
                RoleCode = ur.Role?.Code,
                RoleName = ur.Role?.Name,
                CreatedDate = ur.CreatedDate,
                CreatedBy = ur.CreatedBy,
            });
        }

        public async Task<PaginationResponse<RoleAssignPickerDTO>> GetRolesForAssignAsync(
            Guid userId,
            RoleAssignPickerParam param
        )
        {
            var userRoles = await _userRoleRepository.GetByUserIdAsync(userId);
            var selectedRoleIds = userRoles.Select(ur => ur.RoleId).ToHashSet();

            var allRoles = await _roleRepository.GetListAsync(q =>
            {
                if (!string.IsNullOrWhiteSpace(param.Search))
                    q = q.Where(r =>
                        (r.Name != null && EF.Functions.ILike(r.Name, $"%{param.Search}%"))
                        || (r.Code != null && EF.Functions.ILike(r.Code, $"%{param.Search}%"))
                    );
                return q;
            });

            var dtos = allRoles.Select(r => new RoleAssignPickerDTO
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Name,
                IsSelected = selectedRoleIds.Contains(r.Id),
            });

            bool sortDesc = string.Equals(
                param.SortOrder,
                "desc",
                StringComparison.OrdinalIgnoreCase
            );

            var sorted = sortDesc
                ? dtos.OrderByDescending(r => r.IsSelected)
                    .ThenByDescending(r => r.Name ?? r.Code)
                    .ToList()
                : dtos.OrderByDescending(r => r.IsSelected)
                    .ThenBy(r => r.Name ?? r.Code)
                    .ToList();

            var totalCount = sorted.Count;
            var totalPages =
                totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / param.PageSize);

            var items = sorted
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(
                    (r, i) =>
                    {
                        r.Number = (param.Page - 1) * param.PageSize + i + 1;
                        return r;
                    }
                )
                .ToList();

            return new PaginationResponse<RoleAssignPickerDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPages = totalPages,
            };
        }

        public async Task AssignRolesAsync(Guid userId, AssignRolesParam param)
        {
            var user = await _repository.GetAsync(q => q.Where(u => u.Id == userId));
            if (user is null)
                throw new NotFoundException($"User with ID {userId} not found");

            await _userRoleRepository.BeginTransactionAsync();
            try
            {
                var currentUserRoles = (
                    await _userRoleRepository.GetByUserIdAsync(userId)
                ).ToList();
                var currentRoleIds = currentUserRoles.Select(ur => ur.RoleId).ToHashSet();
                var newRoleIds = param.RoleIds.ToHashSet();
                var actor = _currentUserService.UserName ?? "SYSTEM";
                var now = DateTimeOffset.UtcNow;

                var toRemove = currentUserRoles
                    .Where(ur => !newRoleIds.Contains(ur.RoleId))
                    .ToList();
                foreach (var ur in toRemove)
                {
                    ur.IsDeleted = true;
                    ur.DeletedDate = now;
                    ur.DeletedBy = actor;
                }
                if (toRemove.Count > 0)
                    await _userRoleRepository.UpdateRangeAsync(toRemove);

                var toAdd = newRoleIds
                    .Where(roleId => !currentRoleIds.Contains(roleId))
                    .Select(roleId => new UserRole
                    {
                        UserId = userId,
                        RoleId = roleId,
                        CreatedDate = now,
                        CreatedBy = actor,
                    })
                    .ToList();

                if (toAdd.Count > 0)
                    await _userRoleRepository.CreateRangeAsync(toAdd);

                await _userRoleRepository.CommitTransactionAsync();
            }
            catch
            {
                await _userRoleRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<StatCountDTO> GetTotalStatAsync()
        {
            var total = await _repository.CountAsync();
            return new StatCountDTO { Count = total, Percentage = total > 0 ? 100 : 0 };
        }

        public async Task<StatCountDTO> GetRoleAssignmentStatAsync(bool hasRole)
        {
            var total = await _repository.CountAsync();

            var userRoles = await _userRoleRepository.GetListAsync(q =>
                q.Where(ur => !ur.IsDeleted)
            );
            var withRole = userRoles.Select(ur => ur.UserId).Distinct().Count();

            var count = hasRole ? withRole : Math.Max(total - withRole, 0);
            return new StatCountDTO
            {
                Count = count,
                Percentage = total > 0 ? (int)Math.Round((double)count / total * 100) : 0,
            };
        }
    }
}

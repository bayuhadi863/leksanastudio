using LeksanaStudio.Application.DTOs.Role;
using LeksanaStudio.Application.DTOs.User;
using LeksanaStudio.Application.DTOs.UserRole;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IUserService
        : IBaseCrudService<User, UserDTO, UserParam, UserPaginationDTO, UserPaginationParam>
    {
        Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync(Guid userId);
        Task AssignRolesAsync(Guid userId, AssignRolesParam param);
        Task<PaginationResponse<RoleAssignPickerDTO>> GetRolesForAssignAsync(Guid userId, RoleAssignPickerParam param);
        Task<StatCountDTO> GetTotalStatAsync();
        Task<StatCountDTO> GetRoleAssignmentStatAsync(bool hasRole);
    }
}

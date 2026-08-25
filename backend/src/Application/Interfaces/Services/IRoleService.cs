using LeksanaStudio.Application.DTOs.Menu;
using LeksanaStudio.Application.DTOs.Role;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Common.Models;
using LeksanaStudio.Domain.Entities;

namespace LeksanaStudio.Application.Interfaces.Services
{
    public interface IRoleService
        : IBaseCrudService<Role, RoleDTO, RoleParam, RolePaginationDTO, RolePaginationParam>
    {
        Task<IEnumerable<RoleMenuDTO>> GetRoleMenusAsync(Guid roleId);
        Task AssignMenusAsync(Guid roleId, AssignMenusParam param);
        Task<PaginationResponse<MenuAssignPickerDTO>> GetMenusForAssignAsync(Guid roleId, MenuAssignPickerParam param);
        Task<StatCountDTO> GetTotalStatAsync();
        Task<StatCountDTO> GetMenuAccessStatAsync(bool hasAccess);
    }
}

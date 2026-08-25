using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.Role
{
    public class RoleAssignPickerDTO : BasePaginationItemDTO
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsSelected { get; set; }
    }
}

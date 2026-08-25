using LeksanaStudio.Application.DTOs.Menu;
using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.Role
{
    public class RolePaginationDTO : BasePaginationItemDTO
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Order { get; set; }
        public Guid? DefaultMenuId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public IEnumerable<RoleMenuSummaryDTO> Menus { get; set; } = [];
    }
}

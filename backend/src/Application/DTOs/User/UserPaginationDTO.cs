using LeksanaStudio.Application.DTOs.UserRole;
using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.User
{
    public class UserPaginationDTO : BasePaginationItemDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public IEnumerable<UserRoleSummaryDTO> Roles { get; set; } = [];
    }
}

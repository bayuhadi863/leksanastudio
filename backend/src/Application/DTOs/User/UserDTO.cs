using LeksanaStudio.Application.DTOs.UserRole;

namespace LeksanaStudio.Application.DTOs.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public List<UserRoleDTO> Roles { get; set; } = new List<UserRoleDTO>();
    }
}

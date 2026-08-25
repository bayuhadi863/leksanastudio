namespace LeksanaStudio.Application.DTOs.UserRole
{
    public class UserRoleDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string? RoleCode { get; set; }
        public string? RoleName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}

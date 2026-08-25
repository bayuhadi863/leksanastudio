namespace LeksanaStudio.Application.DTOs.UserRole
{
    public class UserRoleSummaryDTO
    {
        public Guid RoleId { get; set; }
        public string? RoleCode { get; set; }
        public string? RoleName { get; set; }

        /// <summary>Code of the role's default landing menu (null if unset). Drives post-login redirect.</summary>
        public string? DefaultMenuCode { get; set; }
    }
}

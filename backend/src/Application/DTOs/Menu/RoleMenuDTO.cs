namespace LeksanaStudio.Application.DTOs.Menu
{
    public class RoleMenuDTO
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid MenuId { get; set; }
        public string? MenuCode { get; set; }
        public string? MenuName { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanVerify { get; set; }
        public List<string> CustomEvents { get; set; } = [];
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}

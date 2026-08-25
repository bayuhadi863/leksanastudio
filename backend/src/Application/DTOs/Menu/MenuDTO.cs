namespace LeksanaStudio.Application.DTOs.Menu
{
    public class MenuDTO
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        // Merged CRUD permission flags for the current user (only set by user-access).
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanVerify { get; set; }

        /// <summary>Granted custom-event codes (beyond CRUD/verify), e.g. "dashboard-system".</summary>
        public List<string> CustomEvents { get; set; } = [];

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}

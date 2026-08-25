using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.Menu
{
    public class MenuAssignPickerDTO : BasePaginationItemDTO
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsSelected { get; set; }

        // Current per-action permissions for this role↔menu (all false when unselected).
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanVerify { get; set; }

        /// <summary>Custom-event codes this menu supports (e.g. ["verify"]); CRUD is universal.</summary>
        public List<string> CustomEvents { get; set; } = new();
    }
}

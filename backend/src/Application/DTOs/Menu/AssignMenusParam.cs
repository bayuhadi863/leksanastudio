namespace LeksanaStudio.Application.DTOs.Menu
{
    public class AssignMenusParam
    {
        public List<RoleMenuAssignItem> Items { get; set; } = [];

        /// <summary>Landing menu after login/switch. Must be one of the granted (viewable) menus.</summary>
        public Guid? DefaultMenuId { get; set; }
    }

    /// <summary>One menu grant for a role, with its per-action (CRUD) permissions.</summary>
    public class RoleMenuAssignItem
    {
        public Guid MenuId { get; set; }
        public bool CanView { get; set; } = true;
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanVerify { get; set; }

        /// <summary>Granted custom-event codes (excluding verify) — validated against the menu's declared support.</summary>
        public List<string> CustomEvents { get; set; } = [];
    }
}

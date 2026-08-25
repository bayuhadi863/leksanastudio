using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    [Table("role_menu")]
    public class RoleMenu : BaseEntity
    {
        [Column("role_menu_role_id")]
        public Guid RoleId { get; set; }

        [Column("role_menu_menu_id")]
        public Guid MenuId { get; set; }

        // Per-action (CRUD) permission flags. Default true keeps legacy "assigned
        // menu = full access" behaviour; view-only grants set the write flags false.
        [Column("role_menu_canview")]
        public bool CanView { get; set; } = true;

        [Column("role_menu_cancreate")]
        public bool CanCreate { get; set; } = true;

        [Column("role_menu_canupdate")]
        public bool CanUpdate { get; set; } = true;

        [Column("role_menu_candelete")]
        public bool CanDelete { get; set; } = true;

        // Custom event permission (distinct from update) — e.g. verifying submissions.
        [Column("role_menu_canverify")]
        public bool CanVerify { get; set; } = true;

        /// <summary>
        /// Comma-separated granted custom-event codes (beyond CRUD + verify) for this
        /// role-menu, e.g. "dashboard-system". Must be a subset of the menu's declared
        /// SupportedCustomEvents. Null/empty = none.
        /// </summary>
        [MaxLength(255)]
        [Column("role_menu_customevents")]
        public string? CustomEventCodes { get; set; } = null;

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey(nameof(MenuId))]
        public virtual Menu Menu { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    [Table("menu")]
    public class Menu : BaseEntity
    {
        [MaxLength(100)]
        [Column("menu_code")]
        public string? Code { get; set; } = null;

        [MaxLength(255)]
        [Column("menu_name")]
        public string? Name { get; set; } = null;

        /// <summary>
        /// Comma-separated custom-event codes this menu supports beyond the standard
        /// CRUD (e.g. "verify"). Empty/null = only CRUD applies.
        /// </summary>
        [MaxLength(255)]
        [Column("menu_customevents")]
        public string? SupportedCustomEvents { get; set; } = null;
    }
}

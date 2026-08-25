using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    [Table("role")]
    public class Role : BaseEntity
    {
        [MaxLength(100)]
        [Column("role_code")]
        public string? Code { get; set; } = null;

        [MaxLength(255)]
        [Column("role_name")]
        public string? Name { get; set; } = null;

        [Column("role_description")]
        public string? Description { get; set; } = null;

        [Column("role_order")]
        public int? Order { get; set; } = null;

        /// <summary>Menu a user lands on after login / role switch (must be a granted menu).</summary>
        [Column("role_defaultmenu_id")]
        public Guid? DefaultMenuId { get; set; } = null;

        [ForeignKey(nameof(DefaultMenuId))]
        public virtual Menu? DefaultMenu { get; set; } = null;
    }
}

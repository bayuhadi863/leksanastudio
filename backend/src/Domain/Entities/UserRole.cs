using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    [Table("user_role")]
    public class UserRole : BaseEntity
    {
        [Column("user_role_user_id")]
        public Guid UserId { get; set; }

        [Column("user_role_role_id")]
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;
    }
}

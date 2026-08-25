using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    [Table("user_refreshtoken")]
    public class UserRefreshToken : BaseEntity
    {
        [Column("user_refreshtoken_userid")]
        public Guid? UserId { get; set; } = null;

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; } = null;

        [MaxLength(500)]
        [Column("user_refreshtoken_token")]
        public string? Token { get; set; } = null;

        [Column("user_refreshtoken_expiresat")]
        public DateTimeOffset? ExpiresAt { get; set; } = null;

        // Absolute session cap: set once at login, inherited unchanged across refresh
        // rotations. Bounds total session lifetime so an active session can't live forever.
        [Column("user_refreshtoken_absoluteexpiresat")]
        public DateTimeOffset? AbsoluteExpiresAt { get; set; } = null;

        [Column("user_refreshtoken_isrevoked")]
        public bool? IsRevoked { get; set; } = false;
    }
}

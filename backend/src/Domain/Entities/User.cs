using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    [Table("user")]
    public class User : BaseEntity
    {
        [MaxLength(255)]
        [Column("user_name")]
        public string? Name { get; set; } = null;

        [MaxLength(255)]
        [Column("user_email")]
        public string? Email { get; set; } = null;

        /// <summary>BCrypt hash. Never the plaintext — see <c>PasswordHash</c>.</summary>
        [Column("user_password")]
        public string? Password { get; set; } = null;
    }
}

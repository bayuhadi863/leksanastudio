using System.ComponentModel.DataAnnotations;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Domain.Entities.Base
{
    /// <summary>
    /// Base for every persistent entity: identity, soft-delete flag, and the audit
    /// trail. Physical column names are applied per table by a convention in
    /// <c>AppDbContext</c> (<c>{table}_{property}</c>, preserving the legacy schema),
    /// and a global query filter hides soft-deleted rows.
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public bool IsDeleted { get; set; } = false;

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedDate { get; set; } = null;
        public DateTimeOffset? DeletedDate { get; set; } = null;

        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? UpdatedBy { get; set; } = null;

        [MaxLength(100)]
        public string? DeletedBy { get; set; } = null;
    }
}

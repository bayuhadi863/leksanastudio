using System.ComponentModel.DataAnnotations;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Domain.Entities.Base
{
    /// <summary>
    /// Base for every translation row.
    ///
    /// The foreign key lives here as <see cref="ParentId"/> rather than being
    /// re-declared per module: the relationship is configured in
    /// <c>AppDbContext</c>, and physical column names follow the same
    /// <c>{table}_{property}</c> convention as <see cref="BaseEntity"/>.
    /// </summary>
    public abstract class BaseTranslationEntity : BaseEntity, ITranslationEntity
    {
        /// <summary>Owning entry. The pairing between languages runs through this.</summary>
        public Guid ParentId { get; set; }

        [MaxLength(10)]
        public string LocaleCode { get; set; } = string.Empty;

        /// <summary>
        /// The address, not the identity. Free to change; every change is recorded
        /// in the slug history so the old address can still answer with a 301.
        /// </summary>
        [MaxLength(200)]
        public string? Slug { get; set; }

        public ContentStatus Status { get; set; } = ContentStatus.Draft;

        public DateTimeOffset? PublishedAt { get; set; }
    }
}

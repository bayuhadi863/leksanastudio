using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    /// <summary>
    /// Every address a piece of content has ever had.
    ///
    /// Without this, tidying a headline silently kills an indexed URL: the slug
    /// changes, the old one 404s, and nobody notices for three months. With it,
    /// the old address answers 301 and keeps whatever ranking it earned.
    ///
    /// One table for every module — the pair (<see cref="EntityType"/>,
    /// <see cref="EntityId"/>) says which. A per-module table would be more
    /// normalised and would buy nothing: this is only ever read by exact match on
    /// (locale, old slug).
    /// </summary>
    [Table("slug_history")]
    public class SlugHistory : BaseEntity
    {
        /// <summary>Module the slug belonged to, e.g. <c>case-study</c>.</summary>
        [MaxLength(60)]
        [Column("slug_history_entitytype")]
        public string EntityType { get; set; } = string.Empty;

        [Column("slug_history_entityid")]
        public Guid EntityId { get; set; }

        [MaxLength(10)]
        [Column("slug_history_localecode")]
        public string LocaleCode { get; set; } = string.Empty;

        [MaxLength(200)]
        [Column("slug_history_oldslug")]
        public string OldSlug { get; set; } = string.Empty;

        [MaxLength(200)]
        [Column("slug_history_newslug")]
        public string NewSlug { get; set; } = string.Empty;
    }
}

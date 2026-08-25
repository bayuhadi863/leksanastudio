using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    /// <summary>
    /// An uploaded file, stored in object storage and referenced by content.
    ///
    /// Note what is <b>not</b> here: alt text. The same photograph means different
    /// things in different places, so the description belongs at the point of use
    /// — in the block or the field that references it — not on the file.
    /// </summary>
    [Table("media")]
    public class Media : BaseEntity
    {
        /// <summary>Path inside the bucket, e.g. <c>leksana-studio/portfolio/2026-08/…png</c>.</summary>
        [MaxLength(500)]
        [Column("media_objectpath")]
        public string ObjectPath { get; set; } = string.Empty;

        [MaxLength(120)]
        [Column("media_mime")]
        public string Mime { get; set; } = string.Empty;

        [Column("media_sizebytes")]
        public long SizeBytes { get; set; }

        [Column("media_width")]
        public int? Width { get; set; }

        [Column("media_height")]
        public int? Height { get; set; }

        /// <summary>Filename as uploaded — the only handle a human recognises in a picker.</summary>
        [MaxLength(255)]
        [Column("media_originalname")]
        public string OriginalName { get; set; } = string.Empty;

        /// <summary>Free-text label to make the picker searchable. Never rendered as alt text.</summary>
        [MaxLength(255)]
        [Column("media_label")]
        public string? Label { get; set; }
    }
}

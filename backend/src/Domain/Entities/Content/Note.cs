using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>A technical note — the studio's content engine (blueprint 08).</summary>
    [Table("note")]
    public class Note : BaseTranslatableEntity<NoteTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("note_contentkey")]
        public string? ContentKey { get; set; }

        [Column("note_pillar")]
        public NotePillar Pillar { get; set; } = NotePillar.Decision;

        [Column("note_order")]
        public int Order { get; set; }
    }
}

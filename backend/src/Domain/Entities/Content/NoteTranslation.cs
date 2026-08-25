using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("note_translation")]
    public class NoteTranslation : BaseTranslationEntity, IHasBlockBody
    {
        [MaxLength(255)]
        [Column("note_translation_title")]
        public string? Title { get; set; }

        [Column("note_translation_summary")]
        public string? Summary { get; set; }

        [Column("note_translation_body", TypeName = "jsonb")]
        public string? Body { get; set; }
    }
}

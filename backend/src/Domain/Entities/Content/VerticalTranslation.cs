using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("vertical_translation")]
    public class VerticalTranslation : BaseTranslationEntity
    {
        [MaxLength(120)]
        [Column("vertical_translation_industry")]
        public string? Industry { get; set; }

        [Column("vertical_translation_headline")]
        public string? Headline { get; set; }

        [Column("vertical_translation_intro")]
        public string? Intro { get; set; }

        /// <summary>The margin note. First person, carries a limit or an objection — never a feature.</summary>
        [Column("vertical_translation_note")]
        public string? Note { get; set; }

        /// <summary>
        /// Opening WhatsApp message. Differs per page on purpose: it says which page
        /// the prospect came from without anyone having to ask.
        /// </summary>
        [Column("vertical_translation_whatsappintro")]
        public string? WhatsappIntro { get; set; }

        [Column("vertical_translation_problems", TypeName = "jsonb")]
        public string? Problems { get; set; }

        [Column("vertical_translation_deliverables", TypeName = "jsonb")]
        public string? Deliverables { get; set; }

        [Column("vertical_translation_faq", TypeName = "jsonb")]
        public string? Faq { get; set; }
    }
}

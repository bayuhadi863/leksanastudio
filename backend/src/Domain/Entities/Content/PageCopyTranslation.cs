using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("page_copy_translation")]
    public class PageCopyTranslation : BaseTranslationEntity
    {
        /// <summary>
        /// Slot key to value, e.g. <c>{"hero.headline": "…"}</c>. Which keys are
        /// valid comes from the slot definitions, not from whatever was sent.
        /// </summary>
        [Column("page_copy_translation_slots", TypeName = "jsonb")]
        public string? Slots { get; set; }

        [MaxLength(120)]
        [Column("page_copy_translation_metatitle")]
        public string? MetaTitle { get; set; }

        [MaxLength(320)]
        [Column("page_copy_translation_metadescription")]
        public string? MetaDescription { get; set; }
    }
}

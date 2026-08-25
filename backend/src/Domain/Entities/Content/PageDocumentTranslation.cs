using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("page_document_translation")]
    public class PageDocumentTranslation : BaseTranslationEntity, IHasBlockBody
    {
        [MaxLength(255)]
        [Column("page_document_translation_title")]
        public string? Title { get; set; }

        [Column("page_document_translation_lead")]
        public string? Lead { get; set; }

        [Column("page_document_translation_body", TypeName = "jsonb")]
        public string? Body { get; set; }

        [MaxLength(120)]
        [Column("page_document_translation_metatitle")]
        public string? MetaTitle { get; set; }

        [MaxLength(320)]
        [Column("page_document_translation_metadescription")]
        public string? MetaDescription { get; set; }
    }
}

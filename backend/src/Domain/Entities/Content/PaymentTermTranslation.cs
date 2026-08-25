using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("payment_term_translation")]
    public class PaymentTermTranslation : BaseTranslationEntity
    {
        [MaxLength(255)]
        [Column("payment_term_translation_scope")]
        public string? Scope { get; set; }

        [Column("payment_term_translation_schedule")]
        public string? Schedule { get; set; }
    }
}

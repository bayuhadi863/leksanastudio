using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>How payment is staged for one kind of work.</summary>
    [Table("payment_term")]
    public class PaymentTerm : BaseTranslatableEntity<PaymentTermTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("payment_term_contentkey")]
        public string? ContentKey { get; set; }

        [Column("payment_term_order")]
        public int Order { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// An industry landing page — the studio's SEO engine (blueprint 08).
    ///
    /// One template, many rows. A generalist cannot win the head of the search
    /// curve, so the long tail is covered by pages written entirely in one
    /// industry's language. They live at the site root on purpose.
    /// </summary>
    [Table("vertical")]
    public class Vertical : BaseTranslatableEntity<VerticalTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("vertical_contentkey")]
        public string? ContentKey { get; set; }

        [Column("vertical_serviceid")]
        public Guid? ServiceId { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public Service? Service { get; set; }

        [Column("vertical_pricingshape")]
        public PricingShape PricingShape { get; set; } = PricingShape.BusinessPackages;

        [Column("vertical_order")]
        public int Order { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>One of the three lines of work the studio sells.</summary>
    [Table("service")]
    public class Service : BaseTranslatableEntity<ServiceTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("service_contentkey")]
        public string? ContentKey { get; set; }

        /// <summary>Floor price in rupiah. A number, so it can be compared and formatted.</summary>
        [Column("service_startingprice")]
        public decimal StartingPrice { get; set; }

        /// <summary>Decides whether the pricing section renders a package table or a phase list.</summary>
        [Column("service_pricingshape")]
        public PricingShape PricingShape { get; set; } = PricingShape.BusinessPackages;

        /// <summary>
        /// The case study that best matches this line of work. A real relation, not
        /// a slug written by hand — so it cannot dangle when the slug changes.
        /// </summary>
        [Column("service_casestudyid")]
        public Guid? CaseStudyId { get; set; }

        [ForeignKey(nameof(CaseStudyId))]
        public CaseStudy? CaseStudy { get; set; }

        [Column("service_order")]
        public int Order { get; set; }
    }
}

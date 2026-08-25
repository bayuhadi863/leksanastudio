using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>One column of a pricing table.</summary>
    [Table("service_package")]
    public class ServicePackage : BaseTranslatableEntity<ServicePackageTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("service_package_contentkey")]
        public string? ContentKey { get; set; }

        /// <summary>Which comparison table this package appears in.</summary>
        [Column("service_package_group")]
        public PackageGroup Group { get; set; } = PackageGroup.Business;

        /// <summary>Short code shown above the name, e.g. A1.</summary>
        [MaxLength(20)]
        [Column("service_package_code")]
        public string? Code { get; set; }

        [Column("service_package_price")]
        public decimal Price { get; set; }

        /// <summary>
        /// The one package that leads on phones. At most one per group: nobody
        /// scrolls three cards to find the one they should have seen first.
        /// </summary>
        [Column("service_package_highlighted")]
        public bool Highlighted { get; set; }

        [Column("service_package_order")]
        public int Order { get; set; }
    }
}

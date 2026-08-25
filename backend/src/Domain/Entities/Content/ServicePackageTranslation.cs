using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("service_package_translation")]
    public class ServicePackageTranslation : BaseTranslationEntity
    {
        [MaxLength(255)]
        [Column("service_package_translation_name")]
        public string? Name { get; set; }

        [MaxLength(255)]
        [Column("service_package_translation_audience")]
        public string? Audience { get; set; }

        [Column("service_package_translation_summary")]
        public string? Summary { get; set; }

        [MaxLength(255)]
        [Column("service_package_translation_pricenote")]
        public string? PriceNote { get; set; }

        [MaxLength(120)]
        [Column("service_package_translation_duration")]
        public string? Duration { get; set; }

        /// <summary>
        /// Comparison rows. Every package in a group must declare the same labels in
        /// the same order, or the table cannot line up.
        /// </summary>
        [Column("service_package_translation_features", TypeName = "jsonb")]
        public string? Features { get; set; }
    }
}

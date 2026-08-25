using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("service_translation")]
    public class ServiceTranslation : BaseTranslationEntity
    {
        /// <summary>Heading form. Used as a title, never inside a sentence.</summary>
        [MaxLength(255)]
        [Column("service_translation_name")]
        public string? Name { get; set; }

        /// <summary>
        /// Sentence form, lower case. "Lihat layanan {shortName} selengkapnya" has
        /// to read like Indonesian, which a four-word heading does not.
        /// </summary>
        [MaxLength(120)]
        [Column("service_translation_shortname")]
        public string? ShortName { get; set; }

        [MaxLength(255)]
        [Column("service_translation_audience")]
        public string? Audience { get; set; }

        [Column("service_translation_headline")]
        public string? Headline { get; set; }

        [Column("service_translation_summary")]
        public string? Summary { get; set; }

        [MaxLength(120)]
        [Column("service_translation_startingpricelabel")]
        public string? StartingPriceLabel { get; set; }

        /// <summary>Problems specific to this audience. Never generic complaints.</summary>
        [Column("service_translation_problems", TypeName = "jsonb")]
        public string? Problems { get; set; }

        [Column("service_translation_deliverables", TypeName = "jsonb")]
        public string? Deliverables { get; set; }

        /// <summary>What is not included. Written down so nobody has to guess later.</summary>
        [Column("service_translation_exclusions", TypeName = "jsonb")]
        public string? Exclusions { get; set; }

        [Column("service_translation_faq", TypeName = "jsonb")]
        public string? Faq { get; set; }
    }
}

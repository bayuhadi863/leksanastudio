using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("site_profile_translation")]
    public class SiteProfileTranslation : BaseTranslationEntity
    {
        [MaxLength(200)]
        [Column("site_profile_translation_tagline")]
        public string? Tagline { get; set; }

        [MaxLength(400)]
        [Column("site_profile_translation_description")]
        public string? Description { get; set; }

        [MaxLength(120)]
        [Column("site_profile_translation_city")]
        public string? City { get; set; }

        [MaxLength(120)]
        [Column("site_profile_translation_region")]
        public string? Region { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// Who the studio is, and the numbers it promises. Exactly one row.
    ///
    /// This is the single source of truth the whole site reads from: no identity
    /// value and no promise figure may be hard-coded in a component. Changing the
    /// WhatsApp number has to be one edit, not a search across fourteen files —
    /// and it must not require a deploy.
    ///
    /// Locked to super-admin in the panel: these fields are the channel every lead
    /// arrives through, and a typo here costs more than a typo anywhere else.
    /// </summary>
    [Table("site_profile")]
    public class SiteProfile : BaseTranslatableEntity<SiteProfileTranslation>
    {
        [MaxLength(120)]
        [Column("site_profile_name")]
        public string? Name { get; set; }

        [MaxLength(160)]
        [Column("site_profile_legalname")]
        public string? LegalName { get; set; }

        [MaxLength(160)]
        [Column("site_profile_ownername")]
        public string? OwnerName { get; set; }

        [MaxLength(160)]
        [Column("site_profile_email")]
        public string? Email { get; set; }

        /// <summary>International format without the plus sign — used directly in wa.me links.</summary>
        [MaxLength(30)]
        [Column("site_profile_whatsappnumber")]
        public string? WhatsappNumber { get; set; }

        [MaxLength(40)]
        [Column("site_profile_whatsappdisplay")]
        public string? WhatsappDisplay { get; set; }

        [MaxLength(255)]
        [Column("site_profile_linkedin")]
        public string? Linkedin { get; set; }

        [MaxLength(255)]
        [Column("site_profile_instagram")]
        public string? Instagram { get; set; }

        [MaxLength(255)]
        [Column("site_profile_github")]
        public string? Github { get; set; }

        /* Promise figures. They appear in many places; they live in one. */

        [Column("site_profile_revisionrounds")]
        public int RevisionRounds { get; set; } = 2;

        [Column("site_profile_warrantydays")]
        public int WarrantyDays { get; set; } = 60;

        [Column("site_profile_updateeverydays")]
        public int UpdateEveryDays { get; set; } = 3;

        [Column("site_profile_replywithinhours")]
        public int ReplyWithinHours { get; set; } = 2;

        [Column("site_profile_quotevaliddays")]
        public int QuoteValidDays { get; set; } = 14;

        /// <summary>Below this, the only honest work is installing a template. Stated, not hidden.</summary>
        [Column("site_profile_pricefloor")]
        public decimal PriceFloor { get; set; }
    }
}

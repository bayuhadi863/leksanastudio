using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("case_study_translation")]
    public class CaseStudyTranslation : BaseTranslationEntity, IHasBlockBody
    {
        [MaxLength(255)]
        [Column("case_study_translation_title")]
        public string? Title { get; set; }

        [Column("case_study_translation_summary")]
        public string? Summary { get; set; }

        /// <summary>
        /// The problem that was solved, in one sentence. This — not the client's
        /// name — is what the portfolio card leads with: a prospect scans for their
        /// own problem, not for an institution they have never heard of.
        /// </summary>
        [Column("case_study_translation_problem")]
        public string? Problem { get; set; }

        [MaxLength(255)]
        [Column("case_study_translation_client")]
        public string? Client { get; set; }

        [MaxLength(120)]
        [Column("case_study_translation_kind")]
        public string? Kind { get; set; }

        [MaxLength(120)]
        [Column("case_study_translation_duration")]
        public string? Duration { get; set; }

        [Column("case_study_translation_role")]
        public string? Role { get; set; }

        /// <summary>Describes the screenshot for someone who cannot see it. Never a filename.</summary>
        [MaxLength(255)]
        [Column("case_study_translation_coveralt")]
        public string? CoverAlt { get; set; }

        /// <summary>Exactly three headline figures — blueprint 05. Their labels are prose, so they live here.</summary>
        [Column("case_study_translation_metrics", TypeName = "jsonb")]
        public string? Metrics { get; set; }

        [Column("case_study_translation_body", TypeName = "jsonb")]
        public string? Body { get; set; }
    }
}

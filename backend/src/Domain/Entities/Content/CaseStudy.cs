using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// A piece of work presented as evidence.
    ///
    /// The most important content type on the site: a prospect judges the studio
    /// by these before reading a single claim elsewhere.
    /// </summary>
    [Table("case_study")]
    public class CaseStudy : BaseTranslatableEntity<CaseStudyTranslation>, IHasOrder
    {
        /// <summary>Stable handle for entries the code or a seeder refers to by name.</summary>
        [MaxLength(120)]
        [Column("case_study_contentkey")]
        public string? ContentKey { get; set; }

        [Column("case_study_label")]
        public CaseStudyLabel Label { get; set; } = CaseStudyLabel.Client;

        /// <summary>Stand-in drawing, used only while no real screenshot exists.</summary>
        [Column("case_study_figure")]
        public SchematicVariant Figure { get; set; } = SchematicVariant.System;

        [Column("case_study_covermediaid")]
        public Guid? CoverMediaId { get; set; }

        [ForeignKey(nameof(CoverMediaId))]
        public Media? CoverMedia { get; set; }

        [Column("case_study_year")]
        public int Year { get; set; }

        /// <summary>Technology names. Proper nouns, so they read the same in every language.</summary>
        [Column("case_study_stack", TypeName = "jsonb")]
        public string? Stack { get; set; }

        [Column("case_study_order")]
        public int Order { get; set; }
    }
}

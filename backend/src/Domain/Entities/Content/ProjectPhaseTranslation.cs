using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("project_phase_translation")]
    public class ProjectPhaseTranslation : BaseTranslationEntity
    {
        [MaxLength(255)]
        [Column("project_phase_translation_name")]
        public string? Name { get; set; }

        /// <summary>
        /// Written as prose rather than a number: some phases are quoted as a range,
        /// and a range is not a decimal.
        /// </summary>
        [MaxLength(120)]
        [Column("project_phase_translation_price")]
        public string? Price { get; set; }

        [MaxLength(120)]
        [Column("project_phase_translation_duration")]
        public string? Duration { get; set; }

        [Column("project_phase_translation_scope")]
        public string? Scope { get; set; }

        [Column("project_phase_translation_note")]
        public string? Note { get; set; }
    }
}

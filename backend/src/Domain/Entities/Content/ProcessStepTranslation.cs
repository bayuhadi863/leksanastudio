using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("process_step_translation")]
    public class ProcessStepTranslation : BaseTranslationEntity
    {
        [MaxLength(255)]
        [Column("process_step_translation_title")]
        public string? Title { get; set; }

        [MaxLength(120)]
        [Column("process_step_translation_duration")]
        public string? Duration { get; set; }

        [Column("process_step_translation_summary")]
        public string? Summary { get; set; }

        [Column("process_step_translation_details", TypeName = "jsonb")]
        public string? Details { get; set; }

        /// <summary>
        /// What the client has to supply at this step. Projects that slip almost
        /// always slip here, not in the building — so it is stated per step.
        /// </summary>
        [Column("process_step_translation_clientinput")]
        public string? ClientInput { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// One step of how a project runs.
    ///
    /// The least-spoken fear is not price — it is that the process will be a mess
    /// and nobody will know what to do. Each step answers that before it is asked.
    /// </summary>
    [Table("process_step")]
    public class ProcessStep : BaseTranslatableEntity<ProcessStepTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("process_step_contentkey")]
        public string? ContentKey { get; set; }

        [Column("process_step_step")]
        public int Step { get; set; }

        [Column("process_step_order")]
        public int Order { get; set; }
    }
}

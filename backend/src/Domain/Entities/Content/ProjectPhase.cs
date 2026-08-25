using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// One stage of system work.
    ///
    /// System work is sold as a sequence, not as a package list — which is also
    /// why the numbering here is legitimate rather than decorative.
    /// </summary>
    [Table("project_phase")]
    public class ProjectPhase : BaseTranslatableEntity<ProjectPhaseTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("project_phase_contentkey")]
        public string? ContentKey { get; set; }

        [Column("project_phase_step")]
        public int Step { get; set; }

        [Column("project_phase_order")]
        public int Order { get; set; }
    }
}

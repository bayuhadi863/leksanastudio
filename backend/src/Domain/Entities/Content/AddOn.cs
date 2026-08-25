using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// Extra work, priced separately.
    ///
    /// Mentioned only once the main need is settled: listing everything up front
    /// complicates the decision and delays the thing that actually matters.
    /// </summary>
    [Table("add_on")]
    public class AddOn : BaseTranslatableEntity<AddOnTranslation>, IHasOrder
    {
        [MaxLength(120)]
        [Column("add_on_contentkey")]
        public string? ContentKey { get; set; }

        [Column("add_on_order")]
        public int Order { get; set; }
    }
}

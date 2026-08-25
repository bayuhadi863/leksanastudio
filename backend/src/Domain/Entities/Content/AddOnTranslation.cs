using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    [Table("add_on_translation")]
    public class AddOnTranslation : BaseTranslationEntity
    {
        [MaxLength(255)]
        [Column("add_on_translation_name")]
        public string? Name { get; set; }

        [MaxLength(120)]
        [Column("add_on_translation_price")]
        public string? Price { get; set; }

        [MaxLength(255)]
        [Column("add_on_translation_appliesto")]
        public string? AppliesTo { get; set; }
    }
}

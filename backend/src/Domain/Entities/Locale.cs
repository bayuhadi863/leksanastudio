using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities
{
    /// <summary>
    /// A language the site can be published in.
    ///
    /// A table rather than an enum, deliberately: adding a language becomes one
    /// row of data instead of a migration and a deploy, and the panel can list
    /// what exists without being taught about it.
    /// </summary>
    [Table("locale")]
    public class Locale : BaseEntity
    {
        /// <summary>ISO 639-1, e.g. <c>id</c>, <c>en</c>. Referenced by every translation row.</summary>
        [MaxLength(10)]
        [Column("locale_code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>Name as written in the panel's own language.</summary>
        [MaxLength(100)]
        [Column("locale_name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Name as speakers of that language write it — for the public switcher.</summary>
        [MaxLength(100)]
        [Column("locale_nativename")]
        public string NativeName { get; set; } = string.Empty;

        /// <summary>Exactly one locale is the default. Its URLs carry no prefix.</summary>
        [Column("locale_isdefault")]
        public bool IsDefault { get; set; }

        /// <summary>Turns a language off without destroying its translations.</summary>
        [Column("locale_isactive")]
        public bool IsActive { get; set; } = true;

        [Column("locale_order")]
        public int Order { get; set; }
    }
}

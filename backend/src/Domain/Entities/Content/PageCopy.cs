using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// The words on a fixed page — headline, section intros, the margin notes.
    ///
    /// Not a block document, deliberately. The layout of these pages is a design
    /// decision the client does not own (level 3), so what they get is a set of
    /// named slots the developer declared, not a canvas. That is the difference
    /// between a CMS and a page builder.
    /// </summary>
    [Table("page_copy")]
    public class PageCopy : BaseTranslatableEntity<PageCopyTranslation>, IHasOrder
    {
        /// <summary>Which page this belongs to, e.g. <c>home</c>, <c>pricing</c>.</summary>
        [MaxLength(60)]
        [Column("page_copy_pagecode")]
        public string PageCode { get; set; } = string.Empty;

        [Column("page_copy_order")]
        public int Order { get; set; }
    }
}

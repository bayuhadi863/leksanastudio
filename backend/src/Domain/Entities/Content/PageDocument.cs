using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Common.Interfaces;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// A page that is mostly prose — the privacy policy, terms, and their kind.
    ///
    /// Unlike <see cref="PageCopy"/>, these have no fixed section layout, so they
    /// do carry a block body.
    /// </summary>
    [Table("page_document")]
    public class PageDocument : BaseTranslatableEntity<PageDocumentTranslation>, IHasOrder
    {
        [MaxLength(60)]
        [Column("page_document_pagecode")]
        public string PageCode { get; set; } = string.Empty;

        [Column("page_document_order")]
        public int Order { get; set; }
    }
}

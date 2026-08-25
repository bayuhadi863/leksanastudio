using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeksanaStudio.Domain.Entities.Base;

namespace LeksanaStudio.Domain.Entities.Content
{
    /// <summary>
    /// Declares one editable slot on a fixed page: its label, its kind, and how
    /// long it may be.
    ///
    /// Seeded from a manifest in code — the same arrangement <c>MenuSeeder</c>
    /// already uses. Adding a slot is a design decision, so it is a code change;
    /// the panel then builds the form from these rows without being taught about
    /// any particular page.
    /// </summary>
    [Table("page_copy_slot_definition")]
    public class PageCopySlotDefinition : BaseEntity
    {
        [MaxLength(60)]
        [Column("page_copy_slot_definition_pagecode")]
        public string PageCode { get; set; } = string.Empty;

        /// <summary>Dotted key, e.g. <c>hero.headline</c>.</summary>
        [MaxLength(120)]
        [Column("page_copy_slot_definition_slotkey")]
        public string SlotKey { get; set; } = string.Empty;

        /// <summary>What the editor sees above the field.</summary>
        [MaxLength(160)]
        [Column("page_copy_slot_definition_label")]
        public string Label { get; set; } = string.Empty;

        /// <summary>One line under the label saying where this appears and what it is for.</summary>
        [MaxLength(400)]
        [Column("page_copy_slot_definition_hint")]
        public string? Hint { get; set; }

        /// <summary><c>text</c> · <c>textarea</c> · <c>richText</c> · <c>note</c>.</summary>
        [MaxLength(40)]
        [Column("page_copy_slot_definition_kind")]
        public string Kind { get; set; } = "text";

        /// <summary>
        /// Enforced on the server. A slot designed for twelve words will accept
        /// eighty and the page will survive it — but it will not look designed.
        /// </summary>
        [Column("page_copy_slot_definition_maxlength")]
        public int MaxLength { get; set; } = 160;

        [Column("page_copy_slot_definition_required")]
        public bool Required { get; set; }

        /// <summary>Groups slots into sections in the form, e.g. <c>Hero</c>.</summary>
        [MaxLength(120)]
        [Column("page_copy_slot_definition_group")]
        public string? Group { get; set; }

        [Column("page_copy_slot_definition_order")]
        public int Order { get; set; }
    }
}

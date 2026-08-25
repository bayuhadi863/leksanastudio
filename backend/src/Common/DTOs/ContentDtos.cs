using LeksanaStudio.Common.Enums;
using LeksanaStudio.Common.Interfaces;

namespace LeksanaStudio.Common.DTOs
{
    /// <summary>
    /// The fields every translation returns, whatever the module.
    ///
    /// Grouped with its siblings rather than split one-per-file: a module's
    /// contract is easier to check when it can be read in one screen.
    /// </summary>
    public abstract class TranslationDtoBase
    {
        public Guid Id { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public ContentStatus Status { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    /// <summary>The fields every incoming translation carries.</summary>
    public abstract class TranslationParamBase : ITranslationParam
    {
        public string LocaleCode { get; set; } = "id";
        public string? Slug { get; set; }
        public ContentStatus Status { get; set; } = ContentStatus.Draft;
    }

    /// <summary>
    /// How far along each language is for one entry.
    ///
    /// Shown in every list so a half-translated site is visible rather than
    /// discovered — the single most common way a bilingual project fails.
    /// </summary>
    public class TranslationSummaryDTO
    {
        public string LocaleCode { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public ContentStatus Status { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
    }

    /// <summary>Filters every content list supports, on top of the usual paging.</summary>
    public class ContentPaginationParam : BasePaginationParam
    {
        /// <summary>Which language the search term and the title column refer to.</summary>
        public string? LocaleCode { get; set; }

        /// <summary>Narrows to entries that are live, or to those that are not.</summary>
        public ContentStatus? Status { get; set; }
    }

    /// <summary>Shared columns of every content list row.</summary>
    public abstract class ContentPaginationDtoBase : BasePaginationItemDTO
    {
        public Guid Id { get; set; }
        public int Order { get; set; }

        /// <summary>Title in the requested language, so the list reads as the editor expects.</summary>
        public string? Title { get; set; }
        public string? Slug { get; set; }

        public IEnumerable<TranslationSummaryDTO> Translations { get; set; } = [];

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    /// <summary>Ordering payload for the drag-free reorder screens.</summary>
    public class ReorderParam
    {
        public List<Guid> Ids { get; set; } = [];
    }
}

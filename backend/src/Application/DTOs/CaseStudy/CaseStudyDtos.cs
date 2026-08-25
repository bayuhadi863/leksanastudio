using System.Text.Json;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.Application.DTOs.CaseStudy
{
    /* ------------------------------------------------------------------ panel */

    public class CaseStudyDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public CaseStudyLabel Label { get; set; }
        public SchematicVariant Figure { get; set; }
        public Guid? CoverMediaId { get; set; }
        public string? CoverMediaPath { get; set; }
        public int Year { get; set; }

        /// <summary>Technology names. Proper nouns, so the same list serves every language.</summary>
        public JsonElement? Stack { get; set; }

        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<CaseStudyTranslationDTO> Translations { get; set; } = [];
    }

    public class CaseStudyTranslationDTO : TranslationDtoBase
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Problem { get; set; }
        public string? Client { get; set; }
        public string? Kind { get; set; }
        public string? Duration { get; set; }
        public string? Role { get; set; }
        public string? CoverAlt { get; set; }
        public JsonElement? Metrics { get; set; }
        public JsonElement? Body { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class CaseStudyParam : Common.Interfaces.ITranslatableParam<CaseStudyTranslationParam>
    {
        public string? ContentKey { get; set; }
        public CaseStudyLabel Label { get; set; } = CaseStudyLabel.Client;
        public SchematicVariant Figure { get; set; } = SchematicVariant.System;
        public Guid? CoverMediaId { get; set; }
        public int Year { get; set; }
        public JsonElement? Stack { get; set; }
        public int Order { get; set; }

        public List<CaseStudyTranslationParam> Translations { get; set; } = [];
    }

    public class CaseStudyTranslationParam : TranslationParamBase
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Problem { get; set; }
        public string? Client { get; set; }
        public string? Kind { get; set; }
        public string? Duration { get; set; }
        public string? Role { get; set; }
        public string? CoverAlt { get; set; }
        public JsonElement? Metrics { get; set; }
        public JsonElement? Body { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class CaseStudyPaginationDTO : ContentPaginationDtoBase
    {
        public CaseStudyLabel Label { get; set; }
        public int Year { get; set; }

        /// <summary>Mapped from the entity; the path below is resolved from it during enrichment.</summary>
        public Guid? CoverMediaId { get; set; }
        public string? CoverMediaPath { get; set; }
    }

    public class CaseStudyPaginationParam : ContentPaginationParam
    {
        public CaseStudyLabel? Label { get; set; }
        public int? Year { get; set; }
    }

    /* ----------------------------------------------------------- public site */

    /// <summary>
    /// One case study, flattened to a single language, as the public site reads it.
    ///
    /// Drafts never appear here — the endpoint that serves this is anonymous, and
    /// an anonymous endpoint that can return unpublished writing is a leak nobody
    /// notices until it has already happened.
    /// </summary>
    public class CaseStudyPublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public CaseStudyLabel Label { get; set; }
        public SchematicVariant Figure { get; set; }
        public string? CoverPath { get; set; }
        public string? CoverAlt { get; set; }
        public int Year { get; set; }
        public JsonElement? Stack { get; set; }
        public int Order { get; set; }

        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Problem { get; set; }
        public string? Client { get; set; }
        public string? Kind { get; set; }
        public string? Duration { get; set; }
        public string? Role { get; set; }
        public JsonElement? Metrics { get; set; }
        public JsonElement? Body { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

        /// <summary>
        /// This entry's address in every language that has published it. Feeds
        /// hreflang and the language switcher, which is why it is keyed by language
        /// rather than guessed from the slug.
        /// </summary>
        public Dictionary<string, string> Alternates { get; set; } = [];
    }
}

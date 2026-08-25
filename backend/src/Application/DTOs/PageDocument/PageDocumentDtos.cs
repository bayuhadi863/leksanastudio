using System.Text.Json;
using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.PageDocument
{
    /* ------------------------------------------------------------------ panel */

    public class PageDocumentDTO
    {
        public Guid Id { get; set; }

        /// <summary>Stable identifier the code links to, e.g. <c>privacy</c>.</summary>
        public string PageCode { get; set; } = string.Empty;

        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<PageDocumentTranslationDTO> Translations { get; set; } = [];
    }

    public class PageDocumentTranslationDTO : TranslationDtoBase
    {
        public string? Title { get; set; }
        public string? Lead { get; set; }
        public JsonElement? Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class PageDocumentParam
        : Common.Interfaces.ITranslatableParam<PageDocumentTranslationParam>
    {
        public string PageCode { get; set; } = string.Empty;
        public int Order { get; set; }

        public List<PageDocumentTranslationParam> Translations { get; set; } = [];
    }

    public class PageDocumentTranslationParam : TranslationParamBase
    {
        public string? Title { get; set; }
        public string? Lead { get; set; }
        public JsonElement? Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class PageDocumentPaginationDTO : ContentPaginationDtoBase
    {
        public string PageCode { get; set; } = string.Empty;
    }

    public class PageDocumentPaginationParam : ContentPaginationParam { }

    /* ----------------------------------------------------------- public site */

    public class PageDocumentPublicDTO
    {
        public Guid Id { get; set; }
        public string PageCode { get; set; } = string.Empty;
        public string LocaleCode { get; set; } = string.Empty;
        public string? Slug { get; set; }

        public string? Title { get; set; }
        public string? Lead { get; set; }
        public JsonElement? Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

        public Dictionary<string, string> Alternates { get; set; } = [];
    }
}

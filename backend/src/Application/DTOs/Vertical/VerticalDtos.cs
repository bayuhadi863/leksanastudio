using System.Text.Json;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.Application.DTOs.Vertical
{
    /* ------------------------------------------------------------------ panel */

    public class VerticalDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public Guid? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public PricingShape PricingShape { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<VerticalTranslationDTO> Translations { get; set; } = [];
    }

    public class VerticalTranslationDTO : TranslationDtoBase
    {
        public string? Industry { get; set; }
        public string? Headline { get; set; }
        public string? Intro { get; set; }
        public string? Note { get; set; }
        public string? WhatsappIntro { get; set; }
        public JsonElement? Problems { get; set; }
        public JsonElement? Deliverables { get; set; }
        public JsonElement? Faq { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class VerticalParam : Common.Interfaces.ITranslatableParam<VerticalTranslationParam>
    {
        public string? ContentKey { get; set; }
        public Guid? ServiceId { get; set; }
        public PricingShape PricingShape { get; set; } = PricingShape.BusinessPackages;
        public int Order { get; set; }

        public List<VerticalTranslationParam> Translations { get; set; } = [];
    }

    public class VerticalTranslationParam : TranslationParamBase
    {
        public string? Industry { get; set; }
        public string? Headline { get; set; }
        public string? Intro { get; set; }
        public string? Note { get; set; }
        public string? WhatsappIntro { get; set; }
        public JsonElement? Problems { get; set; }
        public JsonElement? Deliverables { get; set; }
        public JsonElement? Faq { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class VerticalPaginationDTO : ContentPaginationDtoBase
    {
        public PricingShape PricingShape { get; set; }
    }

    public class VerticalPaginationParam : ContentPaginationParam
    {
        public Guid? ServiceId { get; set; }
    }

    /* ----------------------------------------------------------- public site */

    /// <summary>One industry page, flattened to a single language. Published only.</summary>
    public class VerticalPublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public PricingShape PricingShape { get; set; }
        public int Order { get; set; }

        public string? Industry { get; set; }
        public string? Headline { get; set; }
        public string? Intro { get; set; }
        public string? Note { get; set; }
        public string? WhatsappIntro { get; set; }
        public JsonElement? Problems { get; set; }
        public JsonElement? Deliverables { get; set; }
        public JsonElement? Faq { get; set; }

        /// <summary>The service this page routes to, by address rather than by id.</summary>
        public string? ServiceSlug { get; set; }
        public string? ServiceName { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

        public Dictionary<string, string> Alternates { get; set; } = [];
    }
}

using System.Text.Json;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.Application.DTOs.Service
{
    /* ------------------------------------------------------------------ panel */

    public class ServiceDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public decimal StartingPrice { get; set; }
        public PricingShape PricingShape { get; set; }

        /// <summary>The case study shown as proof on this service page.</summary>
        public Guid? CaseStudyId { get; set; }

        /// <summary>Title of that case study, so the panel can name it without a second call.</summary>
        public string? CaseStudyTitle { get; set; }

        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<ServiceTranslationDTO> Translations { get; set; } = [];
    }

    public class ServiceTranslationDTO : TranslationDtoBase
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Audience { get; set; }
        public string? Headline { get; set; }
        public string? Summary { get; set; }
        public string? StartingPriceLabel { get; set; }
        public JsonElement? Problems { get; set; }
        public JsonElement? Deliverables { get; set; }
        public JsonElement? Exclusions { get; set; }
        public JsonElement? Faq { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class ServiceParam : Common.Interfaces.ITranslatableParam<ServiceTranslationParam>
    {
        public string? ContentKey { get; set; }
        public decimal StartingPrice { get; set; }
        public PricingShape PricingShape { get; set; } = PricingShape.BusinessPackages;
        public Guid? CaseStudyId { get; set; }
        public int Order { get; set; }

        public List<ServiceTranslationParam> Translations { get; set; } = [];
    }

    public class ServiceTranslationParam : TranslationParamBase
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Audience { get; set; }
        public string? Headline { get; set; }
        public string? Summary { get; set; }
        public string? StartingPriceLabel { get; set; }
        public JsonElement? Problems { get; set; }
        public JsonElement? Deliverables { get; set; }
        public JsonElement? Exclusions { get; set; }
        public JsonElement? Faq { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class ServicePaginationDTO : ContentPaginationDtoBase
    {
        public decimal StartingPrice { get; set; }
        public PricingShape PricingShape { get; set; }
    }

    public class ServicePaginationParam : ContentPaginationParam
    {
        public PricingShape? PricingShape { get; set; }
    }

    /* ----------------------------------------------------------- public site */

    /// <summary>One line of work, flattened to a single language. Published only.</summary>
    public class ServicePublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public decimal StartingPrice { get; set; }
        public PricingShape PricingShape { get; set; }
        public int Order { get; set; }

        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Audience { get; set; }
        public string? Headline { get; set; }
        public string? Summary { get; set; }
        public string? StartingPriceLabel { get; set; }
        public JsonElement? Problems { get; set; }
        public JsonElement? Deliverables { get; set; }
        public JsonElement? Exclusions { get; set; }
        public JsonElement? Faq { get; set; }

        /// <summary>
        /// Address of the proof case study in this language, resolved here rather
        /// than assembled in the browser — a link built from an id would break the
        /// moment the case study's address changed.
        /// </summary>
        public string? CaseStudySlug { get; set; }
        public string? CaseStudyTitle { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

        public Dictionary<string, string> Alternates { get; set; } = [];
    }
}

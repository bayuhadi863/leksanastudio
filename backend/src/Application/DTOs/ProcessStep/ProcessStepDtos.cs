using System.Text.Json;
using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.ProcessStep
{
    /* ------------------------------------------------------------------ panel */

    public class ProcessStepDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public int Step { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<ProcessStepTranslationDTO> Translations { get; set; } = [];
    }

    public class ProcessStepTranslationDTO : TranslationDtoBase
    {
        public string? Title { get; set; }
        public string? Duration { get; set; }
        public string? Summary { get; set; }
        public JsonElement? Details { get; set; }
        public string? ClientInput { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class ProcessStepParam : Common.Interfaces.ITranslatableParam<ProcessStepTranslationParam>
    {
        public string? ContentKey { get; set; }
        public int Step { get; set; } = 1;
        public int Order { get; set; }

        public List<ProcessStepTranslationParam> Translations { get; set; } = [];
    }

    public class ProcessStepTranslationParam : TranslationParamBase
    {
        public string? Title { get; set; }
        public string? Duration { get; set; }
        public string? Summary { get; set; }
        public JsonElement? Details { get; set; }
        public string? ClientInput { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class ProcessStepPaginationDTO : ContentPaginationDtoBase
    {
        public int Step { get; set; }
    }

    public class ProcessStepPaginationParam : ContentPaginationParam { }

    /* ----------------------------------------------------------- public site */

    public class ProcessStepPublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public int Step { get; set; }
        public int Order { get; set; }

        public string? Title { get; set; }
        public string? Duration { get; set; }
        public string? Summary { get; set; }
        public JsonElement? Details { get; set; }
        public string? ClientInput { get; set; }
    }
}

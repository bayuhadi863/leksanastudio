using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.ProjectPhase
{
    /* ------------------------------------------------------------------ panel */

    public class ProjectPhaseDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public int Step { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<ProjectPhaseTranslationDTO> Translations { get; set; } = [];
    }

    public class ProjectPhaseTranslationDTO : TranslationDtoBase
    {
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Duration { get; set; }
        public string? Scope { get; set; }
        public string? Note { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class ProjectPhaseParam
        : Common.Interfaces.ITranslatableParam<ProjectPhaseTranslationParam>
    {
        public string? ContentKey { get; set; }
        public int Step { get; set; } = 1;
        public int Order { get; set; }

        public List<ProjectPhaseTranslationParam> Translations { get; set; } = [];
    }

    public class ProjectPhaseTranslationParam : TranslationParamBase
    {
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Duration { get; set; }
        public string? Scope { get; set; }
        public string? Note { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class ProjectPhasePaginationDTO : ContentPaginationDtoBase
    {
        public int Step { get; set; }
    }

    public class ProjectPhasePaginationParam : ContentPaginationParam { }

    /* ----------------------------------------------------------- public site */

    public class ProjectPhasePublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public int Step { get; set; }
        public int Order { get; set; }

        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Duration { get; set; }
        public string? Scope { get; set; }
        public string? Note { get; set; }
    }
}

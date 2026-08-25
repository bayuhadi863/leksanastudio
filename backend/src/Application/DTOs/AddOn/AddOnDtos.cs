using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.AddOn
{
    /* ------------------------------------------------------------------ panel */

    public class AddOnDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<AddOnTranslationDTO> Translations { get; set; } = [];
    }

    public class AddOnTranslationDTO : TranslationDtoBase
    {
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? AppliesTo { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class AddOnParam : Common.Interfaces.ITranslatableParam<AddOnTranslationParam>
    {
        public string? ContentKey { get; set; }
        public int Order { get; set; }

        public List<AddOnTranslationParam> Translations { get; set; } = [];
    }

    public class AddOnTranslationParam : TranslationParamBase
    {
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? AppliesTo { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class AddOnPaginationDTO : ContentPaginationDtoBase { }

    public class AddOnPaginationParam : ContentPaginationParam { }

    /* ----------------------------------------------------------- public site */

    public class AddOnPublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public int Order { get; set; }

        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? AppliesTo { get; set; }
    }
}

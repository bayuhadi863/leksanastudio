using LeksanaStudio.Common.DTOs;

namespace LeksanaStudio.Application.DTOs.PaymentTerm
{
    /* ------------------------------------------------------------------ panel */

    public class PaymentTermDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<PaymentTermTranslationDTO> Translations { get; set; } = [];
    }

    public class PaymentTermTranslationDTO : TranslationDtoBase
    {
        public string? Scope { get; set; }
        public string? Schedule { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class PaymentTermParam : Common.Interfaces.ITranslatableParam<PaymentTermTranslationParam>
    {
        public string? ContentKey { get; set; }
        public int Order { get; set; }

        public List<PaymentTermTranslationParam> Translations { get; set; } = [];
    }

    public class PaymentTermTranslationParam : TranslationParamBase
    {
        public string? Scope { get; set; }
        public string? Schedule { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class PaymentTermPaginationDTO : ContentPaginationDtoBase { }

    public class PaymentTermPaginationParam : ContentPaginationParam { }

    /* ----------------------------------------------------------- public site */

    public class PaymentTermPublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public int Order { get; set; }

        public string? Scope { get; set; }
        public string? Schedule { get; set; }
    }
}

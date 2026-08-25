using System.Text.Json;
using LeksanaStudio.Common.DTOs;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.Application.DTOs.ServicePackage
{
    /* ------------------------------------------------------------------ panel */

    public class ServicePackageDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public PackageGroup Group { get; set; }
        public string? Code { get; set; }
        public decimal Price { get; set; }
        public bool Highlighted { get; set; }
        public int Order { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }

        public List<ServicePackageTranslationDTO> Translations { get; set; } = [];
    }

    public class ServicePackageTranslationDTO : TranslationDtoBase
    {
        public string? Name { get; set; }
        public string? Audience { get; set; }
        public string? Summary { get; set; }
        public string? PriceNote { get; set; }
        public string? Duration { get; set; }
        public JsonElement? Features { get; set; }
    }

    /* ------------------------------------------------------------------ write */

    public class ServicePackageParam
        : Common.Interfaces.ITranslatableParam<ServicePackageTranslationParam>
    {
        public string? ContentKey { get; set; }
        public PackageGroup Group { get; set; } = PackageGroup.Business;
        public string? Code { get; set; }
        public decimal Price { get; set; }
        public bool Highlighted { get; set; }
        public int Order { get; set; }

        public List<ServicePackageTranslationParam> Translations { get; set; } = [];
    }

    public class ServicePackageTranslationParam : TranslationParamBase
    {
        public string? Name { get; set; }
        public string? Audience { get; set; }
        public string? Summary { get; set; }
        public string? PriceNote { get; set; }
        public string? Duration { get; set; }
        public JsonElement? Features { get; set; }
    }

    /* ------------------------------------------------------------------- list */

    public class ServicePackagePaginationDTO : ContentPaginationDtoBase
    {
        public PackageGroup Group { get; set; }
        public string? Code { get; set; }
        public decimal Price { get; set; }
        public bool Highlighted { get; set; }
    }

    public class ServicePackagePaginationParam : ContentPaginationParam
    {
        public PackageGroup? Group { get; set; }
    }

    /* ----------------------------------------------------------- public site */

    public class ServicePackagePublicDTO
    {
        public Guid Id { get; set; }
        public string? ContentKey { get; set; }
        public string LocaleCode { get; set; } = string.Empty;
        public PackageGroup Group { get; set; }
        public string? Code { get; set; }
        public decimal Price { get; set; }
        public bool Highlighted { get; set; }
        public int Order { get; set; }

        public string? Name { get; set; }
        public string? Audience { get; set; }
        public string? Summary { get; set; }
        public string? PriceNote { get; set; }
        public string? Duration { get; set; }
        public JsonElement? Features { get; set; }
    }
}

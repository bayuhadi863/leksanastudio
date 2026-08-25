using FluentValidation;
using LeksanaStudio.Application.DTOs.ServicePackage;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a pricing package must satisfy before it can be stored.
    ///
    /// A package is read inside a comparison, so its rows have to line up: every
    /// feature needs both a label and a value, or the table renders a gap that
    /// looks like a missing feature rather than a missing edit.
    /// </summary>
    public class ServicePackageRequestValidator : AbstractValidator<ServicePackageParam>
    {
        public ServicePackageRequestValidator()
        {
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Harga tidak boleh negatif.");

            RuleFor(x => x.Code)
                .MaximumLength(20)
                .WithMessage("Kode paket maksimal 20 karakter — ini label pendek di atas nama.");

            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new ServicePackageTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class ServicePackageTranslationValidator
        : AbstractValidator<ServicePackageTranslationParam>
    {
        public ServicePackageTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Nama paket wajib diisi.").MaximumLength(160);

            RuleFor(x => x.Audience)
                .MaximumLength(255)
                .WithMessage("Untuk siapa paket ini maksimal 255 karakter.");

            RuleFor(x => x.Summary).MaximumLength(600);
            RuleFor(x => x.PriceNote).MaximumLength(255);
            RuleFor(x => x.Duration).MaximumLength(120);

            RuleFor(x => x.Features)
                .Must(features => ContentRules.EveryItemHas(features, "label", "value"))
                .WithMessage("Setiap baris perbandingan butuh label dan isinya.");

            RuleFor(x => x.Features)
                .Must(features =>
                    features is { ValueKind: System.Text.Json.JsonValueKind.Array } array
                    && array.GetArrayLength() > 0
                )
                .WithMessage("Isi minimal satu baris perbandingan sebelum paket diterbitkan.")
                .When(x => x.Status == ContentStatus.Published);
        }
    }
}

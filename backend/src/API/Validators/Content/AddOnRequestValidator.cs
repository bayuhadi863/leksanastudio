using FluentValidation;
using LeksanaStudio.Application.DTOs.AddOn;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>What an add-on must satisfy before it can be stored.</summary>
    public class AddOnRequestValidator : AbstractValidator<AddOnParam>
    {
        public AddOnRequestValidator()
        {
            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new AddOnTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class AddOnTranslationValidator : AbstractValidator<AddOnTranslationParam>
    {
        public AddOnTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nama tambahan wajib diisi.")
                .MaximumLength(160);

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage(
                    "Harga wajib diisi sebelum diterbitkan — tambahan tanpa harga adalah pertanyaan, bukan penawaran."
                )
                .MaximumLength(120)
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.AppliesTo).MaximumLength(255);
        }
    }
}

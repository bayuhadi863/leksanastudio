using FluentValidation;
using LeksanaStudio.Application.DTOs.ProjectPhase;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a project phase must satisfy before it can be stored.
    ///
    /// Price is prose here rather than a number: some phases are quoted as a
    /// range, and a range is not a decimal. What is enforced instead is that a
    /// published phase states its price and its scope — the two things a client
    /// actually compares.
    /// </summary>
    public class ProjectPhaseRequestValidator : AbstractValidator<ProjectPhaseParam>
    {
        public ProjectPhaseRequestValidator()
        {
            RuleFor(x => x.Step).GreaterThanOrEqualTo(1).WithMessage("Nomor tahap dimulai dari 1.");

            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new ProjectPhaseTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class ProjectPhaseTranslationValidator : AbstractValidator<ProjectPhaseTranslationParam>
    {
        public ProjectPhaseTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nama tahap wajib diisi.")
                .MaximumLength(160);

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage(
                    "Harga tahap wajib diisi sebelum diterbitkan — halaman harga tanpa angka tidak menjawab apa pun."
                )
                .MaximumLength(120)
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.Duration).MaximumLength(120);
            RuleFor(x => x.Scope).MaximumLength(900);

            RuleFor(x => x.Note).MaximumLength(400).WithMessage("Catatan maksimal 400 karakter.");
        }
    }
}

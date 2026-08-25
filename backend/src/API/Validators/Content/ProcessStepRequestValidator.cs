using FluentValidation;
using LeksanaStudio.Application.DTOs.ProcessStep;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a process step must satisfy before it can be stored.
    ///
    /// A published step has to say what the client supplies. Projects rarely slip
    /// in the building; they slip waiting for content, access, or an approval that
    /// nobody knew was needed — so the field is not optional once the page is live.
    /// </summary>
    public class ProcessStepRequestValidator : AbstractValidator<ProcessStepParam>
    {
        public ProcessStepRequestValidator()
        {
            RuleFor(x => x.Step).GreaterThanOrEqualTo(1).WithMessage("Nomor langkah dimulai dari 1.");

            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new ProcessStepTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class ProcessStepTranslationValidator : AbstractValidator<ProcessStepTranslationParam>
    {
        public ProcessStepTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Judul langkah wajib diisi.")
                .MaximumLength(160);

            RuleFor(x => x.Duration).MaximumLength(120);
            RuleFor(x => x.Summary).MaximumLength(600);

            RuleFor(x => x.Details)
                .Must(ContentRules.EveryItemIsText)
                .WithMessage("Setiap rincian harus berupa satu baris, dan tidak boleh kosong.");

            RuleFor(x => x.ClientInput)
                .NotEmpty()
                .WithMessage(
                    "Isi apa yang perlu disiapkan klien di langkah ini — di sinilah proyek biasanya tertahan."
                )
                .MaximumLength(600)
                .When(x => x.Status == ContentStatus.Published);
        }
    }
}

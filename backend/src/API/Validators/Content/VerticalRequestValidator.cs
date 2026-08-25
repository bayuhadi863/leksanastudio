using FluentValidation;
using LeksanaStudio.Application.DTOs.Vertical;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What an industry page must satisfy before it can be stored.
    ///
    /// These pages exist to be found by someone searching in their own industry's
    /// words, which only works if the page is written in those words. The rules
    /// that follow are the minimum that keeps a vertical from becoming the same
    /// page with one noun swapped.
    /// </summary>
    public class VerticalRequestValidator : AbstractValidator<VerticalParam>
    {
        public VerticalRequestValidator()
        {
            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new VerticalTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class VerticalTranslationValidator : AbstractValidator<VerticalTranslationParam>
    {
        public VerticalTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Industry)
                .NotEmpty()
                .WithMessage("Nama industri wajib diisi.")
                .MaximumLength(120);

            RuleFor(x => x.Headline)
                .NotEmpty()
                .WithMessage("Judul halaman wajib diisi sebelum diterbitkan.")
                .MaximumLength(280)
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.Intro).MaximumLength(900);

            RuleFor(x => x.Note)
                .MaximumLength(400)
                .WithMessage("Catatan pinggir maksimal 400 karakter — di atas itu ia berhenti jadi catatan.");

            RuleFor(x => x.WhatsappIntro)
                .MaximumLength(280)
                .WithMessage("Pembuka WhatsApp maksimal 280 karakter.");

            RuleFor(x => x.Problems)
                .Must(ContentRules.EveryItemIsText)
                .WithMessage("Setiap masalah harus berupa satu kalimat, dan tidak boleh kosong.");

            RuleFor(x => x.Problems)
                .Must(items =>
                    items is { ValueKind: System.Text.Json.JsonValueKind.Array } array
                    && array.GetArrayLength() > 0
                )
                .WithMessage(
                    "Isi minimal satu masalah khas industri ini sebelum diterbitkan — tanpa itu halaman ini hanya salinan."
                )
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.Deliverables)
                .Must(ContentRules.EveryItemIsText)
                .WithMessage("Setiap keluaran harus berupa satu baris, dan tidak boleh kosong.");

            RuleFor(x => x.Faq)
                .Must(faq => ContentRules.EveryItemHas(faq, "question", "answer"))
                .WithMessage("Setiap pertanyaan umum butuh pertanyaan dan jawabannya.");

            RuleFor(x => x.Slug)
                .Must(ContentRules.BeAWellFormedSlug)
                .WithMessage(ContentRules.SlugMessage)
                .When(x => !string.IsNullOrWhiteSpace(x.Slug));
        }
    }
}

using System.Text.Json;
using FluentValidation;
using LeksanaStudio.Application.DTOs.CaseStudy;
using LeksanaStudio.Common.Content;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a case study must satisfy before it can be stored.
    ///
    /// The rules here are not house style — they are the blueprint made
    /// executable. Exactly three metrics (05), an alt text that describes rather
    /// than names, and at least one language present. Anything the panel lets
    /// through, this refuses.
    /// </summary>
    public class CaseStudyRequestValidator : AbstractValidator<CaseStudyParam>
    {
        public CaseStudyRequestValidator()
        {
            RuleFor(x => x.Year)
                .InclusiveBetween(2000, 2100)
                .WithMessage("Tahun tidak masuk akal. Isi antara 2000 dan 2100.");

            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations)
                .NotEmpty()
                .WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new CaseStudyTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class CaseStudyTranslationValidator : AbstractValidator<CaseStudyTranslationParam>
    {
        public CaseStudyTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Judul wajib diisi.")
                .MaximumLength(160)
                .WithMessage("Judul maksimal 160 karakter — di atas itu kartu portofolio pecah.");

            RuleFor(x => x.Summary)
                .NotEmpty()
                .WithMessage("Ringkasan wajib diisi.")
                .MaximumLength(420);

            // The portfolio card leads with this, so it has to be one sentence a
            // prospect can recognise as their own problem.
            RuleFor(x => x.Problem)
                .NotEmpty()
                .WithMessage("Masalah wajib diisi — ini kalimat yang muncul di kartu portofolio.")
                .MaximumLength(280)
                .WithMessage("Masalah maksimal 280 karakter. Satu kalimat, bukan paragraf.");

            RuleFor(x => x.Client).MaximumLength(160);
            RuleFor(x => x.Kind).MaximumLength(80);
            RuleFor(x => x.Duration).MaximumLength(80);
            RuleFor(x => x.Role).MaximumLength(400);

            RuleFor(x => x.CoverAlt)
                .MinimumLength(10)
                .WithMessage(
                    "Deskripsi gambar perlu menjelaskan isinya, bukan menamainya. Minimal 10 karakter."
                )
                .When(x => !string.IsNullOrWhiteSpace(x.CoverAlt));

            RuleFor(x => x.Metrics)
                .Must(HaveExactlyThreeMetrics)
                .WithMessage(
                    $"Metrik harus tepat {BlockLimits.MetricsCount} angka — tidak dua, tidak empat."
                )
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.Slug)
                .Must(ContentRules.BeAWellFormedSlug)
                .WithMessage(ContentRules.SlugMessage)
                .When(x => !string.IsNullOrWhiteSpace(x.Slug));
        }

        private static bool HaveExactlyThreeMetrics(JsonElement? metrics) =>
            metrics is { ValueKind: JsonValueKind.Array } array
            && array.GetArrayLength() == BlockLimits.MetricsCount;
    }
}

using FluentValidation;
using LeksanaStudio.Application.DTOs.PageDocument;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a prose page must satisfy before it can be stored.
    ///
    /// These are the pages a visitor reaches when they are already suspicious —
    /// the privacy policy, the terms. A title and a body are the minimum; the meta
    /// description is capped so the search result is not truncated mid-promise.
    /// </summary>
    public class PageDocumentRequestValidator : AbstractValidator<PageDocumentParam>
    {
        public PageDocumentRequestValidator()
        {
            RuleFor(x => x.PageCode)
                .NotEmpty()
                .WithMessage("Kode halaman wajib diisi — ini yang ditunjuk oleh tautan di footer.")
                .MaximumLength(60);

            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new PageDocumentTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class PageDocumentTranslationValidator : AbstractValidator<PageDocumentTranslationParam>
    {
        public PageDocumentTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Judul wajib diisi.")
                .MaximumLength(200);

            RuleFor(x => x.Lead).MaximumLength(600);

            RuleFor(x => x.MetaTitle)
                .MaximumLength(120)
                .WithMessage("Judul meta maksimal 120 karakter — di atas itu terpotong di hasil pencarian.");

            RuleFor(x => x.MetaDescription)
                .MaximumLength(320)
                .WithMessage("Deskripsi meta maksimal 320 karakter.");

            RuleFor(x => x.Body)
                .Must(body =>
                    body is { ValueKind: System.Text.Json.JsonValueKind.Array } array
                    && array.GetArrayLength() > 0
                )
                .WithMessage("Isi tulisannya sebelum halaman ini diterbitkan.")
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.Slug)
                .Must(ContentRules.BeAWellFormedSlug)
                .WithMessage(ContentRules.SlugMessage)
                .When(x => !string.IsNullOrWhiteSpace(x.Slug));
        }
    }
}

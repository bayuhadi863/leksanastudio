using FluentValidation;
using LeksanaStudio.Application.DTOs.Note;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a note must satisfy before it can be stored.
    ///
    /// A note exists to be specific about one decision. The summary is required
    /// for that reason: it is the sentence that says which decision, and a note
    /// nobody can place from its summary will not be read.
    /// </summary>
    public class NoteRequestValidator : AbstractValidator<NoteParam>
    {
        public NoteRequestValidator()
        {
            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new NoteTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class NoteTranslationValidator : AbstractValidator<NoteTranslationParam>
    {
        public NoteTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Judul wajib diisi.")
                .MaximumLength(160)
                .WithMessage("Judul maksimal 160 karakter — di atas itu kartu catatan pecah.");

            RuleFor(x => x.Summary)
                .NotEmpty()
                .WithMessage("Ringkasan wajib diisi — ini kalimat yang menentukan catatan ini dibaca atau tidak.")
                .MaximumLength(400);

            RuleFor(x => x.Slug)
                .Must(ContentRules.BeAWellFormedSlug)
                .WithMessage(ContentRules.SlugMessage)
                .When(x => !string.IsNullOrWhiteSpace(x.Slug));
        }
    }
}

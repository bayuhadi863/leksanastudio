using FluentValidation;
using LeksanaStudio.Application.DTOs.Service;
using LeksanaStudio.Common.Enums;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a service must satisfy before it can be stored.
    ///
    /// The rules that matter here are the promises: a published service page names
    /// its audience, says what is delivered, and states a floor price. A page
    /// missing any of those sends the prospect to WhatsApp to ask — which is the
    /// conversation the page exists to prevent.
    /// </summary>
    public class ServiceRequestValidator : AbstractValidator<ServiceParam>
    {
        public ServiceRequestValidator()
        {
            RuleFor(x => x.StartingPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Harga mulai tidak boleh negatif.");

            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new ServiceTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class ServiceTranslationValidator : AbstractValidator<ServiceTranslationParam>
    {
        public ServiceTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nama layanan wajib diisi.")
                .MaximumLength(160);

            RuleFor(x => x.ShortName)
                .MaximumLength(120)
                .WithMessage("Nama pendek maksimal 120 karakter.");

            RuleFor(x => x.Audience)
                .NotEmpty()
                .WithMessage("Untuk siapa layanan ini wajib diisi — halaman tanpa audiens tidak menjual apa pun.")
                .MaximumLength(255)
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.Headline).MaximumLength(280);
            RuleFor(x => x.Summary).MaximumLength(600);
            RuleFor(x => x.StartingPriceLabel).MaximumLength(120);

            RuleFor(x => x.Problems)
                .Must(ContentRules.EveryItemIsText)
                .WithMessage("Setiap masalah harus berupa satu kalimat, dan tidak boleh kosong.");

            RuleFor(x => x.Deliverables)
                .Must(ContentRules.EveryItemIsText)
                .WithMessage("Setiap keluaran harus berupa satu baris, dan tidak boleh kosong.");

            RuleFor(x => x.Deliverables)
                .Must(items => items is { ValueKind: System.Text.Json.JsonValueKind.Array } array && array.GetArrayLength() > 0)
                .WithMessage("Isi minimal satu keluaran sebelum diterbitkan — ini yang dibeli klien.")
                .When(x => x.Status == ContentStatus.Published);

            RuleFor(x => x.Exclusions)
                .Must(ContentRules.EveryItemIsText)
                .WithMessage("Setiap batasan harus berupa satu baris, dan tidak boleh kosong.");

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

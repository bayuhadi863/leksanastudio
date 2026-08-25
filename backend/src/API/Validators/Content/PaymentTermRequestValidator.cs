using FluentValidation;
using LeksanaStudio.Application.DTOs.PaymentTerm;

namespace LeksanaStudio.API.Validators.Content
{
    /// <summary>
    /// What a payment term must satisfy before it can be stored.
    ///
    /// Both fields are required, always — a term whose scope or schedule is blank
    /// is not a term, and this is the one table on the site a client may later
    /// hold the studio to.
    /// </summary>
    public class PaymentTermRequestValidator : AbstractValidator<PaymentTermParam>
    {
        public PaymentTermRequestValidator()
        {
            RuleFor(x => x.Order).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Translations).NotEmpty().WithMessage("Isi minimal satu bahasa.");

            RuleForEach(x => x.Translations).SetValidator(new PaymentTermTranslationValidator());

            RuleFor(x => x.Translations)
                .Must(ContentRules.OneTranslationPerLocale)
                .WithMessage("Satu bahasa hanya boleh muncul sekali.");
        }
    }

    public class PaymentTermTranslationValidator : AbstractValidator<PaymentTermTranslationParam>
    {
        public PaymentTermTranslationValidator()
        {
            RuleFor(x => x.LocaleCode).NotEmpty().WithMessage("Bahasa wajib dipilih.");

            RuleFor(x => x.Scope)
                .NotEmpty()
                .WithMessage("Untuk pekerjaan apa termin ini berlaku wajib diisi.")
                .MaximumLength(255);

            RuleFor(x => x.Schedule)
                .NotEmpty()
                .WithMessage("Jadwal pembayaran wajib diisi.")
                .MaximumLength(600);
        }
    }
}

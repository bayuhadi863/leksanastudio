using FluentValidation;
using LeksanaStudio.Application.DTOs.User;

namespace LeksanaStudio.API.Validators.User
{
    public class UserRequestValidator : AbstractValidator<UserParam>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .Length(3, 100)
                .WithMessage("Name must be between 3 and 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email address");

            // Optional on purpose: the same param serves updates, where a blank password
            // means "keep the current one". Being required on create is enforced by
            // UserService.OnCreating, which is the only place that can tell the two apart.
            // The minimum matches the create form (UserSchema.ts).
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Kata sandi minimal 8 karakter")
                .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}

using FluentValidation;
using LeksanaStudio.Application.DTOs.Auth;

namespace LeksanaStudio.API.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginParam>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}

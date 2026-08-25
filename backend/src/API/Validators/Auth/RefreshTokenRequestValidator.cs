using FluentValidation;
using LeksanaStudio.Application.DTOs.Auth;

namespace LeksanaStudio.API.Validators.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenParam>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}

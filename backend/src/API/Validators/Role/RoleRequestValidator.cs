using FluentValidation;
using LeksanaStudio.Application.DTOs.Role;

namespace LeksanaStudio.API.Validators.Role
{
    public class RoleRequestValidator : AbstractValidator<RoleParam>
    {
        public RoleRequestValidator()
        {
            RuleFor(x => x.Code)
                .MaximumLength(100)
                .WithMessage("Code must not exceed 100 characters")
                .When(x => x.Code != null);

            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage("Name must not exceed 255 characters")
                .When(x => x.Name != null);

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Order must be greater than or equal to 0")
                .When(x => x.Order != null);
        }
    }
}

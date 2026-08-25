using FluentValidation;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Validators
{
    public class BaseRequestValidator<T> : AbstractValidator<BaseRequest<T>>
    {
        public BaseRequestValidator(IValidator<T> innerValidator)
        {
            RuleFor(x => x.Data)
                .NotNull()
                .WithMessage("Data is required")
                .SetValidator(innerValidator!);
        }
    }
}

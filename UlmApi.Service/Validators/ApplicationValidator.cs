using FluentValidation;
using UlmApi.Domain.Entities;

namespace UlmApi.Service.Validators
{
    public class ApplicationValidator : AbstractValidator<Application>
    {
        public ApplicationValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Application name is required.")
                .NotNull().WithMessage("Application name is required.");
        }
    }
}

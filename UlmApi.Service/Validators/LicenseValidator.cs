using FluentValidation;
using UlmApi.Domain.Entities;

namespace UlmApi.Service.Validators
{
    public class LicenseValidator : AbstractValidator<License>
    {
        public LicenseValidator()
        {
            RuleFor(p => p.Key)
                .NotEmpty().WithMessage("Key is required.")
                .NotNull().WithMessage("Key is required.");

            RuleFor(p => p.Label)
                .NotEmpty().WithMessage("Label is required.")
                .NotNull().WithMessage("Label is required.");

            RuleFor(p => p.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("The Quantity field must be at least equal to 1.")
                .NotNull().WithMessage("The Quantity field must be at least equal to 1.");

            RuleFor(p => p.SolutionId)
                .GreaterThanOrEqualTo(1).WithMessage("Solution is required.")
                .NotNull().WithMessage("Solution is required.");

            RuleFor(p => p.AquisitionDate)
                .NotEmpty().WithMessage("AquisitionDate is required.");

            RuleFor(p => p.ExpirationDate)
                .NotEmpty().WithMessage("ExpirationDate is required.");
        }
    }
}
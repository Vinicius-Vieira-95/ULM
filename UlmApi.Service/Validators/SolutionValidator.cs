using FluentValidation;
using UlmApi.Domain.Entities;

namespace UlmApi.Service.Validators
{
    public class SolutionValidator : AbstractValidator<Solution>
    {
        public SolutionValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull().WithMessage("Name is required.");
            
            RuleFor(u => u.ProductId)
                .GreaterThanOrEqualTo(1).WithMessage("Product is required.")
                .NotNull().WithMessage("Product is required.");
            
            RuleFor(u => u.OwnerId)
                .NotEmpty().WithMessage("Owner is required.")
                .NotNull().WithMessage("Owner is required.");
        }
    }
}

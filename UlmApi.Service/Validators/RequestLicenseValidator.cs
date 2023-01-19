using FluentValidation;
using UlmApi.Domain.Entities;

namespace UlmApi.Service.Validators
{
    public class RequestLicenseValidator : AbstractValidator<RequestLicense>
    {
        public RequestLicenseValidator()
        {
            RuleFor(p => p.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("The Quantity field must be at least equal to 1.")
                .NotNull().WithMessage("The Quantity field must be at least equal to 1.");

            RuleFor(p => p.UsageTime)
                .IsInEnum().WithMessage("This value don't exist in usage time.")
                .NotNull().WithMessage("Usage time is required.");

            RuleFor(p => p.ProductId)
                .GreaterThanOrEqualTo(1).WithMessage("Product is required.")
                .NotNull().WithMessage("Product is required.");

            RuleFor(p => p.ApplicationId)
                .GreaterThanOrEqualTo(1).WithMessage("Application is required.")
                .NotNull().WithMessage("Application is required.");

            RuleFor(p => p.RequesterId)
                .NotEmpty().WithMessage("Requester is required.")
                .NotNull().WithMessage("Requester is required.");
            
            RuleFor(p => p.Justification)
                .NotEmpty().WithMessage("Justification is required.")
                .NotNull().WithMessage("Justification is required.");
            
            RuleFor(p => p.SolutionId)
                .GreaterThanOrEqualTo(1).WithMessage("Solution is required.")
                .NotNull().WithMessage("Solution is required.");
            
            RuleFor(p => p.Reason)
                .IsInEnum().WithMessage("It is necessary to put a reason")
                .NotNull().WithMessage("It is necessary to put a reason");
        }
    }
}